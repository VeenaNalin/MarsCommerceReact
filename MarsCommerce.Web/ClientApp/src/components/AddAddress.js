import React, { useState } from 'react';
import axios from 'axios';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from './UnAuthorized';
import secureLocalStorage from "react-secure-storage";

function AddAddress() {
    const [address1, setaddress1] = useState('');
    const [address2, setaddress2] = useState('');
    const [city, setcity] = useState('');
    const [state, setstate] = useState('');
    const [pincode, setpincode] = useState();
    const [mobile, setmobile] = useState();
    const [isdefaultaddress, setisdefaultaddress] = useState(false);
    const reactBaseURL = process.env.REACT_APP_REACT_URL+ process.env.REACT_APP_ADDRESSES;
    const userId = secureLocalStorage.getItem("userId");
    let base_updateaddress_Url = process.env.REACT_APP_ADD_UPDATE_ADDRESS;
    let base_Url = process.env.REACT_APP_BASE_URL;


    const handleaddress1 = (event) => setaddress1(event.target.value)
    const handleaddress2 = (event) => setaddress2(event.target.value)
    const handlecity = (event) => setcity(event.target.value)
    const handlestate = (event) => setstate(event.target.value)
    const handlepincode = (event) => {
        const value = event.target.value.replace(/\D/g, '').slice(0, 6);
        setpincode(value);
    };
    const handlemobile = (event) => {
        const value = event.target.value.replace(/\D/g, '').slice(0, 10);
        setmobile(value);
    };
    const handleisdefaultaddress = (event) => {
        if (event.target.checked) {
            setisdefaultaddress("true");
        }
        else {
            setisdefaultaddress("false")
        }
    }


    const ClearFormData = () => {
        setaddress1('');
        setaddress2('');
        setcity('');
        setstate('');
        setpincode('');
        setmobile('');
        setisdefaultaddress(false);
       
    }

    const SaveData = () => {
        const data = {
            id: 0,
            userId: secureLocalStorage.getItem("userId"),
            address1: address1,
            address2: address2,
            city: city,
            state: state,
            pinCode: parseInt(pincode),
            mobile: parseInt(mobile),
            isDefaultAddress: JSON.parse(isdefaultaddress)
           
        }
       

        
        try
        {
            axios.post(process.env.REACT_APP_API_BASE_URL + process.env.REACT_APP_ADD_ADDRESS, data)
                .then((response) =>
                {
                    if (!isdefaultaddress) {
                        alert("Address Saved");
                        ClearFormData();
                        window.location.href = reactBaseURL;
                    }
                    else {
                        UpdateDefault(userId, response.data.id);
                        alert("Address Saved");
                        ClearFormData();
                        window.location.href = reactBaseURL;

                    }

                })
                .catch(error =>
                {
                    console.error(error);
                });
            
           
        }
        catch (error)
        {
        }
    }
    const UpdateDefault = (userId, addressId) => {
        try {
            //post default addresses
            const jsonData = {
                userId: userId,
                addressId: addressId,
            };
            axios.post(base_Url + base_updateaddress_Url, jsonData)
                .then(res => res            
            );
        } catch (error) {
        }
    }

    return (
         <>
            <AuthenticatedTemplate>
        <div>            
            
            <form className="row g-3 needs-validation">
                <div className="col-md-10">
                    <h3>Add Address</h3>
                    <div>
                        <div className="col-md-12">
                            <label htmlFor="validationcustom01" className="form-label">Address1</label>
                            <div className="col-lg-4">
                                <input value={address1} type="text" onChange={handleaddress1} id="validationcustom01" className="form-control" required />
                                <div className="invalid-feedback">
                                    please provide a valid address1
                                </div>
                            </div>
                        </div>
                        <div className="col-md-12">
                            <label htmlFor="validationcustom02" className="form-label">Address2</label>
                            <div className="col-lg-4">
                                <input value={address2} type="text" onChange={handleaddress2} id="validationcustom02" className="form-control" required />
                                <div className="invalid-feedback">
                                    please provide a valid address2
                                </div>
                            </div>
                        </div>
                        <div className="col-md-12">
                            <label htmlFor="validationcustom03" className="form-label">City</label>
                            <div className="col-lg-4">
                                <input value={city} type="text" onChange={handlecity} id="validationcustom03" className="form-control" required />
                                <div className="invalid-feedback">
                                    please provide a valid city
                                </div>
                            </div>
                        </div>
                        <div className="col-md-12">
                            <label htmlFor="validationcustom04" className="form-label">State</label>
                            <div className="col-lg-4">
                                <input value={state} type="text" onChange={handlestate} id="validationcustom04" className="form-control" required />
                                <div className="invalid-feedback">
                                    please provide a valid state
                                </div>
                            </div>
                        </div>
                                <div className="col-md-12">
                                    <label htmlFor="validationcustom05" className="form-label">Pincode</label>
                                    <div className="col-lg-4">
                                        <input value={pincode} type="text" onChange={handlepincode} id="validationcustom05" className="form-control" pattern="[0-9]{6}" required />
                                        <div className="invalid-feedback">
                                            Please provide a valid 6-digit pincode
                                        </div>
                                    </div>
                                </div>
                                <div className="col-md-12">
                                    <label htmlFor="validationcustom06" className="form-label">Mobile</label>
                                    <div className="col-lg-4">
                                        <input value={mobile} type="tel" onChange={handlemobile} id="validationcustom06" className="form-control" maxLength="10" required />
                                        <div className="invalid-feedback">
                                            Please provide a valid 10 digit mobile
                                        </div>
                                    </div>
                                </div>
                        <input value={isdefaultaddress} type="checkbox" checked={isdefaultaddress} onChange={handleisdefaultaddress} /><label>&nbsp;&nbsp;Default Address</label><br />
                        <div className="col-12">
                            <button className="btn btn-primary" type="submit" onClick={SaveData}>Save</button>
                        </div>

                    </div>
                </div>
               
            </form >
           
                </div>
            </AuthenticatedTemplate >
            <UnauthenticatedTemplate>
                <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
            </UnauthenticatedTemplate>
        </>
    );
}
export default AddAddress;