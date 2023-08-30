import { useState } from "react";
import ListAddress from "./ListAddress";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from './UnAuthorized';
import secureLocalStorage from "react-secure-storage";

const Addresses = () => {
    const userId = secureLocalStorage.getItem("userId");
    const [selectedAddress, setSelectedAddress] = useState(0);
    const react_base_url = process.env.REACT_APP_REACT_URL;

    const sendDataToParentAddress = (index) => {
        setSelectedAddress(index);

    };

    return (
       <>
            <AuthenticatedTemplate>
            <h3>Your addresses</h3>
            <hr />
                <ListAddress userId={userId} sendDataToParentAddress={sendDataToParentAddress} />
            <br />
            <div className="container">
                <div className="row rounded-3">
                    <div className="col">
                        <form className="px-md-2">
                            <div className="no-gutters col-2"   >
                                <button className="btn btn-success " style={{ backgroundColor: "orange", color: "black" }}
                                    onClick={(e) => {
                                        e.preventDefault(); window.location.href = react_base_url + 'Address'
                                    }}>Add Address</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            </AuthenticatedTemplate >
            <UnauthenticatedTemplate>
                <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
            </UnauthenticatedTemplate>
        </>
    );
}
export default Addresses;