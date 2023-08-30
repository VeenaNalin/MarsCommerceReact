import React, { useEffect, useState } from 'react';
import './OrderDetails.css';
import axios from 'axios';
import {
    BrowserRouter as Router, Link,
    useLocation
} from "react-router-dom";
import DisplayImage from '../ImageComponents/DisplayImage';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from '../UnAuthorized';

function OrderDetails() {
    const api_base_url = process.env.REACT_APP_API_BASE_URL;
    const api_order_detail_url = process.env.REACT_APP_ORDERGET_BYID_API;
    const [totalPrice, setTotalPrice] = useState(0);
    const [order, setOrders] = useState([]);
    const [orderDetail, setOrdersDetails] = useState([]);
    const [paymentInfo, setpaymentInfo] = useState('');
    const { search } = useLocation();
    const id = new URLSearchParams(search).get("id");
    const otherAmounts = 0;

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

        axios.get(api_base_url + api_order_detail_url + id)
            .then(res => res)
            .then(
                (result) => {
                    const orderItem = result.data.items;
                    setOrders(result.data);
                    setOrdersDetails(orderItem);
                    const totalresult = result.data.items.reduce((total, currentValue) => total = total + currentValue.totalPrice, 0);
                    setTotalPrice(totalresult);
                    setpaymentInfo(result.data.paymentInfo.paymentMethod);
                }
            );
    }, []);
    return (
        <>
            <AuthenticatedTemplate>
                <section className="h-100 gradient-custom">
                    <div className="container py-5 h-100">
                        <div className="row d-flex justify-content-center align-items-center h-100">
                            <div className="col-lg-10 col-xl-8">
                                <div className="card" style={{ borderradius: 10 }}>
                                    <div className="card-header px-5 py-3">
                                        <h5 className="text-muted mb-0">Thanks for your Order</h5>
                                    </div>

                                    <div className="d-flex justify-content-between pt-2 px-5">
                                        <p className="fw-bold mb-0 summ-head1">Order Details</p>
                                    </div>

                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">Order Number :{order.id}</p>
                                    </div>

                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">Order Date : {FormatDateTimeVal(order.orderDate)}</p>
                                    </div>

                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">Delivery Date : {FormatDateTimeValDateOnly(order.deliveryDate)}</p>
                                    </div>

                                    {orderDetail.map((ord, index) => (
                                        <div key={index} className="card-body p-3">

                                            <div className="card shadow-0 border mb-6">
                                                <div className="card-body">
                                                    <div className="row">

                                                        <div className="col-md-4">
                                                            <div className="image-size" >
                                                                <DisplayImage ImageUrl={ord.product.imageUrl} />
                                                            </div>

                                                        </div>
                                                        <div className="col-md-6 ms-3">
                                                            <span className="mb-0 text-price fw-bold">{ord.product.name}</span>
                                                            <br></br>
                                                            <span className="text-descriptions fw-normal">{ord.product.description}</span>
                                                            <br></br>
                                                            <span className="text-descriptions fw-normal">Qty:</span>
                                                            <span className="text-descriptions fw-normal">{ord.quantity}</span>
                                                            <br></br>
                                                            <span className="text-descriptions fw-normal">Price:</span>
                                                            <span className="text-descriptions fw-normal">{ord.unitPrice.toFixed(2)}</span>
                                                            <br></br>
                                                            <span className="text-descriptions fw-normal" > Total Price :</span>
                                                            <span className="text-descriptions fw-bold" style={{ float: "right" }} >{ord.totalPrice.toFixed(2)}</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    ))}

                                    <div className="d-flex justify-content-between pt-2 px-5">
                                        <p className="fw-bold mb-0 summ-head1">Payment method</p>
                                    </div>
                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">{paymentInfo}</p>
                                    </div>

                                    <div className="d-flex justify-content-between pt-2 px-5">
                                        <p className="fw-bold mb-0 summ-head1">Order Summary</p>
                                    </div>

                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">Discount :</p>
                                        <span className="text-descriptions fw-bold" style={{ float: "right" }} >{otherAmounts.toFixed(2)}</span>
                                    </div>

                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">Postage and Packing :</p>
                                        <span className="text-descriptions fw-bold" style={{ float: "right" }} >{otherAmounts.toFixed(2)}</span>
                                    </div>

                                    <div className="d-flex justify-content-between px-5">
                                        <p className="text-muted mb-0 summ-head2">Total Amount : </p>
                                        <span className="text-descriptions fw-bold" style={{ float: "right", color: "red" }} >{totalPrice.toFixed(2)}</span>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </AuthenticatedTemplate>
            <UnauthenticatedTemplate>
                <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
            </UnauthenticatedTemplate>
        </>
    )
}
export default OrderDetails;