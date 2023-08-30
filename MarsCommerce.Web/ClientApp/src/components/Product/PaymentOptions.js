import React, { useState, useEffect } from "react";
import axios from 'axios';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from '../UnAuthorized';

function PaymentOptions({ sendDataToParent }) {
    const [payment, setPayment] = useState([]);

    const [checked, setChecked] = useState(0);

    useEffect(() => {
        var PaymentUrl = process.env.REACT_APP_BASE_URL + process.env.REACT_APP_GET_PAYMENTDETAILS;
        axios.get(PaymentUrl).then(response => {
            setPayment(response.data);

        });
    }, []);
    const handleRadio = (Id) => {
        setChecked(Id);
    };


    return (
        <>
            <AuthenticatedTemplate>
                <div className="container ">
                    <section>
                        <div className="row card">
                            <div className=" position-static">

                                <div className="card-body">

                                    {payment.map((type, index) => (
                                        <div key={index} className="d-flex flex-row pb-3">
                                            <div className="d-flex align-items-center pe-2">
                                                <input
                                                    key={index}
                                                    type="radio"
                                                    value={type.id}
                                                    checked={checked === type.id}
                                                    
                                                    onChange={() => {
                                                        handleRadio(type.id);   sendDataToParent(type);
                                                    }} //this is where it needs to be passed

                                                />
                                            </div>
                                            <div className=" align-items-center">
                                                <p className="mb-0">
                                                    <i className="fab fa-cc-visa fa-lg text-primary pe-2"></i>
                                                    <label className='ms-1 label-brand'>
                                                        <span style={{ color: "black", fontSize: 18 }}>{type.paymentMethod}</span>
                                                    </label>
                                                </p>
                                            </div>
                                        </div>

                                    ))}


                                </div>
                            </div>
                        </div>

                    </section>
                </div>
            </AuthenticatedTemplate >
            <UnauthenticatedTemplate>
                <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
            </UnauthenticatedTemplate>
        </>
    )
}
export default PaymentOptions;