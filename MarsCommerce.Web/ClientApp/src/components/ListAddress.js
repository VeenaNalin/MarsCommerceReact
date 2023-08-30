import React, { useEffect, useState } from 'react';
import {
    BrowserRouter as Router, Link,
    useLocation
} from "react-router-dom";
import axios from 'axios';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from './UnAuthorized';

function ListAddress({ userId, sendDataToParentAddress }) {
    const [listAddress, setlistAddress] = useState([]);
    const [selectAddress, setSelectedAddress] = useState();
    const [defaultAddress, setDefaultAddress] = useState([]);
    const id = userId;
    const [isClicked, setIsCliked] = useState(false);
    let base_Url = process.env.REACT_APP_BASE_URL;
    let base_updateaddress_Url = process.env.REACT_APP_ADD_UPDATE_ADDRESS;
    let load_ur = process.env.REACT_APP_LIST_ADDRESS + id;
    let updateDefualt_Url = process.env.REACT_APP_ADDRESS_SET_DEFAULT;


    useEffect(() => {
        axios.get(base_Url + load_ur)
            .then(res => res)
            .then(
                (result) => {
                    setlistAddress(result.data);
                    if (result.data.length > 0) {
                        setSelectedAddress(result.data.filter(item => item.isDefaultAddress === true)[0].id);
                        sendDataToParentAddress(result.data.filter(item => item.isDefaultAddress === true)[0].id);
                    }
                }
            );

    }, [isClicked]);
    const UpdateDefault = (userId, addressId) => {
        try {
            //post default addresses
            const jsonData = {
                userId: userId,
                addressId: addressId,
            };
            axios.post(base_Url + base_updateaddress_Url, jsonData)
                .then(res => res)
                .then(

                    alert('Successfully Changed')

                );

        } catch (error) {
        }
    }
    const UpdateSelected = (userId, addressId) => {
        try {
            setSelectedAddress(addressId);
        } catch (error) {
        }
    };

    return (
        <>
            <AuthenticatedTemplate>
                <div className="container ">
                    <div className="row rounded-3">

                        <div className="col">
                            <form className="px-md-2">

                                <div className="border-dark">

                                    <div className="">
                                        {
                                            listAddress.map((item) => (


                                                <div key={item.id} className="address-line">

                                                    <div className="form-check form-radio">


                                                        <div className="address-details">



                                                            <div className="fw-bold fs-6">
                                                                <input
                                                                    className="form-check-input"
                                                                    name="defaultAddress"
                                                                    //onChange={() => UpdateDefault(item.userId, item.id)}
                                                                    type="radio"
                                                                    value={item.id}
                                                                    defaultChecked={selectAddress === item.id}
                                                                    onClick={() => {
                                                                        sendDataToParentAddress(item.id); UpdateSelected(item.userId, item.id)
                                                                    }} //this is where it needs to be passed
                                                                />
                                                                {`${item.address1},
                                                                    ${item.address2},
                                                                    ${item.city}, 
                                                                    ${item.state},
                                                                    ${item.pinCode},
                                                                    ${item.mobile}`}

                                                            </div>

                                                        </div>

                                                    </div>


                                                </div>

                                            ))
                                        }
                                    </div>

                                </div >
                                <div className="no-gutters col-2"   >
                                    {listAddress.length > 0 ?
                                        <button className="btn btn-success " style={{ backgroundColor: "orange", color: "black" }} onClick={() => {
                                            UpdateDefault(userId, selectAddress)
                                        }} >Make it Default</button> : null}
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
export default ListAddress;
