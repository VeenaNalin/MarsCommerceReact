import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import UnAuthorized from '../UnAuthorized';
import './Order.css';
import secureLocalStorage from "react-secure-storage";


function OrderSummary() {
    const api_base_url = process.env.REACT_APP_API_BASE_URL;
    const api_order_url = process.env.REACT_APP_ORDERGET_BYUSER_API;
    const react_base_url = process.env.REACT_APP_REACT_URL;
    const [orders, setOrders] = useState([]);
    const userId = secureLocalStorage.getItem("userId");


    const FormatDateTimeVal = (data) => {
        const now = new Date(data);
        const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
        const formattedDateWithOptions = now.toLocaleString('en-US', options); // Example format: July 3, 2023, 12:34:56 AM
        return formattedDateWithOptions;
    };

    const FormatDateTimeValDateOnly = (data) => {
        const now = new Date(data);
        const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        const formattedDateWithOptions = now.toLocaleString('en-US', options); // Example format: July 3, 2023, 12:34:56 AM
        return formattedDateWithOptions;
    };


    useEffect(() => {
        axios.get(api_base_url + api_order_url + userId)
            .then(res => res)
            .then(
                (result) => {
                    const orderItem = result.data;
                    setOrders(orderItem);
                }
            );
    }, []);

    return (
        <>
            <AuthenticatedTemplate>
                <div className="container my-5 py-5">
                    <section>
                        <div className="row">
                            <div className=" position-static">
                                <div className="card">
                                    <div className="card-header py-1" style={{ backgroundColor: "gray" }} >
                                        <h5 className="mb-0 text-font" style={{ color: "white" }} >Your Orders </h5>
                                    </div>

                                    <div className="card-body">
                                        {orders.map((order, index) => (
                                            <div key={index} className="row card" onClick={(e) => { e.preventDefault(); window.location.href = react_base_url + 'OrderDetails?id=' + order.id; }} >


                                                <div className="col-md-6 ms-3">
                                                    <span className="text-descriptions fw-normal">Ordered On:</span>
                                                    <span className="mb-0 text-price fw-bold">{FormatDateTimeVal(order.orderDate)}</span>
                                                    <br></br>
                                                    <span className="text-descriptions fw-normal">Delivery On:</span>
                                                    <span className="text-descriptions fw-normal">{FormatDateTimeValDateOnly(order.deliveryDate)}</span>
                                                    <br></br>
                                                    <span className="text-descriptions fw-normal">Items Ordered:</span>
                                                    <span className="text-descriptions fw-normal">{order.items.length}</span>
                                                    <br></br>

                                                </div>
                                            </div>
                                        ))}

                                    </div>

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
export default OrderSummary;