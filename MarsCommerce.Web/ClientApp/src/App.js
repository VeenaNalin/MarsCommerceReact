import React, { useEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { MsalProvider, useMsal } from '@azure/msal-react';
import { EventType } from '@azure/msal-browser';
import { compareIssuingPolicy } from './utils/claimUtils';
import { b2cPolicies } from './authConfig';
import ProductListing from './components/ProductListing';
import { useIsAuthenticated } from "@azure/msal-react";
import axios from "axios";
import {
    InteractionRequiredAuthError,
    InteractionStatus,
} from "@azure/msal-browser";
import secureLocalStorage from "react-secure-storage";

const Pages = () => {
    const baseURL = process.env.REACT_APP_BASE_URL;
    const { instance, inProgress, accounts } = useMsal();
    const [apiData, setApiData] = useState(null);

    /**
     * useMsal is hook that returns the PublicClientApplication instance,
     * an array of all accounts currently signed in and an inProgress value
     * that tells you what msal is currently doing. For more, visit:
     * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/hooks.md
     */
    //const { instance } = useMsal();
    const isAuthenticated = useIsAuthenticated();
    useEffect(() => {
        const callbackId = instance.addEventCallback((event) => {
            if (
                (event.eventType === EventType.LOGIN_SUCCESS || event.eventType === EventType.ACQUIRE_TOKEN_SUCCESS) &&
                event.payload.account
            ) {
                /**
                 * For the purpose of setting an active account for UI update, we want to consider only the auth
                 * response resulting from SUSI flow. "tfp" claim in the id token tells us the policy (NOTE: legacy
                 * policies may use "acr" instead of "tfp"). To learn more about B2C tokens, visit:
                 * https://docs.microsoft.com/en-us/azure/active-directory-b2c/tokens-overview
                 */
                if (compareIssuingPolicy(event.payload.idTokenClaims, b2cPolicies.names.editProfile)) {
                    // retrieve the account from initial sing-in to the app
                    const originalSignInAccount = instance
                        .getAllAccounts()
                        .find(
                            (account) =>
                                account.idTokenClaims.oid === event.payload.idTokenClaims.oid &&
                                account.idTokenClaims.sub === event.payload.idTokenClaims.sub &&
                                compareIssuingPolicy(account.idTokenClaims, b2cPolicies.names.signUpSignIn)
                        );

                    let signUpSignInFlowRequest = {
                        authority: b2cPolicies.authorities.signUpSignIn.authority,
                        account: originalSignInAccount,
                    };

                    // silently login again with the signUpSignIn policy
                    instance.ssoSilent(signUpSignInFlowRequest);
                }

                /**
                 * Below we are checking if the user is returning from the reset password flow.
                 * If so, we will ask the user to reauthenticate with their new password.
                 * If you do not want this behavior and prefer your users to stay signed in instead,
                 * you can replace the code below with the same pattern used for handling the return from
                 * profile edit flow
                 */
                if (compareIssuingPolicy(event.payload.idTokenClaims, b2cPolicies.names.forgotPassword)) {
                    let signUpSignInFlowRequest = {
                        authority: b2cPolicies.authorities.signUpSignIn.authority,

                    };
                    instance.loginRedirect(signUpSignInFlowRequest);
                }


                //Inserting user registration
                let activeAccount;
                let userRole = 'User'
                secureLocalStorage.setItem('isAdmin', false);

                if (instance) {
                    activeAccount = instance.getActiveAccount();

                    //set user data for registration
                    if (activeAccount.idTokenClaims.extension_UserRole != null) {
                        userRole = activeAccount.idTokenClaims.extension_UserRole;
                        secureLocalStorage.setItem('isAdmin', true);
                    }
                    
                    var data = {
                        firstName: activeAccount.idTokenClaims.given_name,
                        lastName: activeAccount.idTokenClaims.family_name,
                        azureUserId: activeAccount.localAccountId,
                        authToken: activeAccount.idToken,
                        userRole: userRole
                    };
                    //Add and Update user Token and User Details
                    axios.post(baseURL + process.env.REACT_APP_REGISTER_USER, data).then((response) => {
                        secureLocalStorage.setItem('userId', response.data.id);
                        secureLocalStorage.setItem('userRole', userRole);
                        
                    }, []);
                }
                const accessTokenRequest = {
                    scopes: [...protectedResources.apiscope.scopes.read, ...protectedResources.apiscope.scopes.write],
                    account: instance.getActiveAccount(),
                };
                instance
                    .acquireTokenSilent(accessTokenRequest)
                    .then((accessTokenResponse) => {
                        // Acquire token silent success
                        let accessToken = accessTokenResponse.accessToken;
                        // Call your API with token
                        //callApi(accessToken).then((response) => {
                        //    setApiData(response);
                        //});
                        setApiData(accessToken);
                        secureLocalStorage.setItem('accessToken', accessToken);
                      
                       
                        axios.defaults.headers.common['Authorization'] = `Bearer ${secureLocalStorage.getItem('accessToken')}`;
                       
                    })
                    .catch((error) => {
                        if (error instanceof InteractionRequiredAuthError) {
                            instance
                                .acquireTokenPopup(accessTokenRequest)
                                .then(function (accessTokenResponse) {
                                    // Acquire token interactive success
                                    let accessToken = accessTokenResponse.accessToken;
                                    // Call your API with token
                                    //callApi(accessToken).then((response) => {
                                    //    setApiData(response);
                                    //});
                                    setApiData(accessToken);
                                   
                                    secureLocalStorage.setItem('accessToken', accessToken);
                                    
                                })
                                .catch(function (error) {
                                    // Acquire token interactive failure
                                    console.log(error);
                                });
                        }
                        console.log(error);
                    });
            }

            if (event.eventType === EventType.LOGIN_FAILURE) {
                // Check for forgot password error
                // Learn more about AAD error codes at https://docs.microsoft.com/en-us/azure/active-directory/develop/reference-aadsts-error-codes
                if (event.error && event.error.errorMessage.includes('AADB2C90118')) {
                    const resetPasswordRequest = {
                        authority: b2cPolicies.authorities.forgotPassword.authority,
                        scopes: [],
                    };
                    instance.loginRedirect(resetPasswordRequest);
                }
            }
        });
        const protectedResources = {
            apiscope: {
                endpoint: 'http://localhost:44462/apitoken',
                scopes: {
                    read: ['https://marscad.onmicrosoft.com/apitoken/demo.read'],
                    write: ['https://marscad.onmicrosoft.com/apitoken/demo.write'],
                },
            },
        };
        
        return () => {
            if (callbackId) {
                instance.removeEventCallback(callbackId);
            }
        };
        // eslint-disable-next-line
    }, [instance]);

    return (
        //isAuthenticated ?
        <Routes>

            {
                AppRoutes.map((route, index) => {
                    const { element, ...rest } = route;
                    return <Route key={index} {...rest} element={element} />;
                })
            }

        </Routes >
        /*:
        <Routes>

            return <Route path='/' element={<ProductListing />} />;
  
        </Routes > */



    );
};
function App({ instance }) {
    // static displayName = App.name;

    return (
        <MsalProvider instance={instance}>
            <Layout>
                <Pages></Pages>
            </Layout>
        </MsalProvider>
    );

}
export default App;

