import { InteractionStatus } from "@azure/msal-browser";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import {
    Collapse, DropdownItem, DropdownMenu, DropdownToggle, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink,
    UncontrolledDropdown
} from 'reactstrap';
import axios from 'axios';
import { b2cPolicies, loginRequest } from '../authConfig';
import './NavMenu.css';
import secureLocalStorage from "react-secure-storage";

export const NavMenu = () => {
    const { instance, inProgress } = useMsal();
    const [collapsed, setcollapsed] = useState(false);
    const [cartCount, secartCount] = useState(0);
    let isAdmin = secureLocalStorage.getItem("isAdmin");

    let activeAccount;
    if (instance) {
        activeAccount = instance.getActiveAccount();
        console.log(secureLocalStorage.getItem("isAdmin"))
       
    }
    const api_base_url = process.env.REACT_APP_API_BASE_URL;
    const api_get_url = process.env.REACT_APP_GET_CARTITEM;
    const userId = secureLocalStorage.getItem("userId");
    axios.defaults.headers.common['Authorization'] = `Bearer ${secureLocalStorage.getItem('accessToken')}`;
    const handleLoginPopup = () => {
        instance
            .loginPopup({
                ...loginRequest,
                redirectUri: '',
            })
            .catch((error) => console.log(error));
    };

    const handleLoginRedirect = () => {
        instance.loginRedirect(loginRequest).catch((error) => console.log(error));
    };

    const handleLogoutRedirect = () => {
        instance.logoutRedirect();
        secureLocalStorage.removeItem("userId");
        secureLocalStorage.removeItem("userRole");
        secureLocalStorage.removeItem("isAdmin");
    };

    const handleLogoutPopup = () => {
        instance.logoutPopup({
            mainWindowRedirectUri: '/', // redirects the top level app after logout
            
        });
        secureLocalStorage.clear();
    };

    const handleProfileEdit = () => {
        if (inProgress === InteractionStatus.None) {
            instance.acquireTokenRedirect(b2cPolicies.authorities.editProfile);
        }
    };
    const handletoggleNavbar = (event) => setcollapsed(!collapsed);
    const LoadFormData = () => {
        axios.get(api_base_url + api_get_url + userId)
            .then(res => {
                secartCount(res.data.userId > 0 ? res.data.items.length : 0)
            }).catch(error => {
                console.log(error);
            })
       
    }


    //Get Form load data
    useEffect(() => {
        LoadFormData();
    }, []);

    return (
        <>
            <header>
                <Navbar className="navbar navbar-dark  navbar-expand-lg navbar-light bg-dark" container light>
                    <span><img style={{ width: 70, height: 60, }} src="./MarsCommerceLogo.png" ></img></span>
                    <NavbarBrand className="navbar-brand text-light " style={{ fontFamily: "Cursive" }} tag={Link} to="/">MarsCommerce</NavbarBrand>

                    <NavbarToggler onClick={handletoggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" data-target="#navbarNav" data-toggle="collapse" isOpen={!collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <AuthenticatedTemplate>
                                <NavItem>
                                    <NavLink tag={Link} className="text-light" to="/cart">
                                        <img style={{ width: 40, height: 40 }} src="./CartIcon.png" alt="" />
                                        <span className="badge badge-warning">{cartCount}</span>
                                    </NavLink>
                                </NavItem>
                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        <img style={{ width: 40, height: 40 }} src="./AdminIcon.png" alt="Admin" />

                                    </DropdownToggle>

                                    <DropdownMenu>
                                        {isAdmin === true ? <DropdownItem tag={Link} className="text-dark" to="/addproduct">Add Product</DropdownItem>
                                            : null}
                                        <DropdownItem tag={Link} className="text-dark" to="/OrderSummary">Your Orders</DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>



                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        <img style={{ height: 40 }}></img>
                                        {activeAccount && activeAccount.idTokenClaims.given_name ? 'Hi, ' + activeAccount.idTokenClaims.given_name : 'Hello, sign in'}

                                    </DropdownToggle>
                                    <DropdownMenu>
                                        <DropdownItem as="button" onClick={handleProfileEdit}>
                                            Edit profile
                                        </DropdownItem>
                                        <DropdownItem as="button" tag={Link} to="/addresses">
                                            Your addresses
                                        </DropdownItem>
                                        <DropdownItem as="button" onClick={handleLogoutPopup}>
                                            Sign out
                                        </DropdownItem>
                                        {/*<DropdownItem as="button" onClick={handleLogoutRedirect}>*/}
                                        {/*    Sign out using popup*/}
                                        {/*</DropdownItem>*/}
                                    </DropdownMenu>
                                </UncontrolledDropdown>


                                {/*<button type="submit" className="btn btn-success " style={{ marginleft: 150, with: 20, height: 40, backgroundColor: "black", color: "white" }} onClick={handleProfileEdit} >Edit Profile</button>*/}

                            </AuthenticatedTemplate>
                            <UnauthenticatedTemplate>
                                <div className="collapse navbar-collapse justify-content-end">
                                    <UncontrolledDropdown nav inNavbar>
                                        <DropdownToggle nav caret  >

                                            <img style={{ width: 40, height: 40 }} src="./AdminIcon.png" alt="Admin" />
                                        </DropdownToggle>
                                        <DropdownMenu variant="secondary" className="ml-auto" drop="start" title="Sign In" end>

                                            <DropdownItem as="button" onClick={handleLoginPopup}>
                                                Sign in
                                            </DropdownItem>
                                            {/*<DropdownItem as="button" onClick={handleLoginRedirect}>*/}
                                            {/*    Sign in using Redirect*/}
                                            {/*</DropdownItem>*/}
                                        </DropdownMenu>
                                    </UncontrolledDropdown>

                                </div>
                            </UnauthenticatedTemplate>
                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        </>
    );
};
