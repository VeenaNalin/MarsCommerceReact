import React, { useEffect, useState } from 'react';
import './Order.css';
import ListAddress from '../ListAddress';
import axios from 'axios';
import DisplayImage from '../ImageComponents/DisplayImage';
import PaymentOptions from '../Product/PaymentOptions';
import { render } from '../../../node_modules/react-dom/cjs/react-dom.development';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from '../UnAuthorized';
import secureLocalStorage from "react-secure-storage";

function OrderCreation() {
    const api_base_url = process.env.REACT_APP_API_BASE_URL;
    const api_getcart_url = process.env.REACT_APP_GET_CARTITEM;
    const api_createorder_url = process.env.REACT_APP_ORDERCREATE_API
    const [shoppingCart, setshoppingCart] = useState([]);
    const [totalPrice, setTotalPrice] = useState(0);
    const [itemData, setItemsData] = useState([]);
    const userId = secureLocalStorage.getItem("userId");
    const [showResultsAddress, setShowResultsAddress] = React.useState(true);
    const [showResultsItem, setShowResultsItem] = React.useState(true)
    const [showResultsPayment, setShowResultsPayment] = React.useState(true)
    const [selectedAddress, setSelectedAddress] = useState(0);
    const [selectedPayment, setSelectedPayment] = useState(0);

    useEffect(() => {
        axios.get(api_base_url + api_getcart_url + userId)
            .then(res => res)
            .then(
                (result) => {
                    const cartItem = result;
                    setshoppingCart(cartItem.data.items);
                    const totalresult = cartItem.data.items.reduce((total, currentValue) => total = total + currentValue.totalPrice, 0);
                    setTotalPrice(totalresult);
                    setItemsData([])
                    for (let i = 0; i < cartItem.data.items.length; i++) {


                        var temp = {
                            id: 0,
                            orderId: 0,
                            productId: cartItem.data.items[i].product.id,
                            quantity: cartItem.data.items[i].quntity,
                            unitPrice: cartItem.data.items[i].unitPrice,
                            totalPrice: cartItem.data.items[i].totalPrice
                        }

                        AddItem(temp)




                    };
                }
            );
    }, []);
    const sendDataToParent = (index) => { // the callback. Use a better name
        setSelectedPayment(index.id);

    };
    const sendDataToParentAddress = (index) => { // the callback. Use a better name
        setSelectedAddress(index);

    };

    const getCurrentDateTime = (addDays) => {
        var currentdate = new Date();
        var datetime = addZero(currentdate.getFullYear()) + "-" +
            addZero(currentdate.getMonth() + 1) + "-" +
            addZero(currentdate.getDate() + addDays) + "T" +
            addZero(currentdate.getHours()) + ":" +
            addZero(currentdate.getMinutes()) + ":" +
            addZero(currentdate.getSeconds());
        return datetime;
    }

    function addZero(str) {
        return str < 10 ? ('0' + str) : str;
    }

    const handleOrderSubmit = () => {

        if (selectedAddress > 0 && selectedPayment > 0) {
            const data = {
                id: 0,
                userId: userId,
                orderDate: getCurrentDateTime(0),//"required->2023-06-13T01:08:49.451Z , getting->2023-07-06 12:16:51",
                paymentInfoId: selectedPayment,
                deliveryCharge: 0,
                deliveryDate: getCurrentDateTime(3),//delivery date added 3 days to now()
                addressId: selectedAddress,
                orderTotal: totalPrice,
                items: itemData
            };
           
            try {
                axios.post(api_base_url + api_createorder_url, data)
                    .then((response) => {
                        alert('Order Booked Successfully');
                        window.location.href = '/';
                    })
                    ;
            } catch (error) {
            }
        }
        else {
            alert("Please select a valid address and payment method");
        }


    };
    const AddItem = (newItem) => {
        setItemsData(prev => [...prev, newItem]);
    };

    const onClickAddress = () => setShowResultsAddress(bool => !bool)
    const onClickItem = () => setShowResultsItem(bool => !bool)
    const onClickPayment = () => setShowResultsPayment(bool => !bool)
    return (
        <>
            <AuthenticatedTemplate>
                <div className="container my-5 py-5">


                    <section>
                        <div className="row">
                            <div className=" position-static">
                                <div className="card  ">
                                    <div onClick={onClickAddress} className="card-header py-1" style={{ backgroundColor: "gray" }}  >
                                        <h5 data-toggle="collapse" style={{ color: "white" }} data-target="#collapseExample" className="mb-0 text-font">Delivery Address </h5>
                                    </div>
                                    <div >
                                        {showResultsAddress ? <div className="card card-body" id="collapseExample" >
                                            <ListAddress userId={userId} sendDataToParentAddress={sendDataToParentAddress} />
                                        </div> : null}
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div className="row">
                            <div className=" position-static">
                                <div className="card">
                                    <div onClick={onClickItem} className="card-header py-1" style={{ backgroundColor: "gray" }} >
                                        <h5 className="mb-0 text-font" style={{ color: "white" }} >Items:{shoppingCart.length} </h5>
                                    </div>
                                    {showResultsItem ?
                                        <div className="card-body">
                                            {shoppingCart.map((cart, index) => (
                                                <div key={index} className="row">
                                                    <div className="col-md-4">
                                                        <div className="image-size" >
                                                            <DisplayImage ImageUrl={cart.product.imageUrl} />
                                                        </div>

                                                    </div>
                                                    <div className="col-md-6 ms-3">
                                                        <span className="mb-0 text-price fw-bold">{cart.product.name}</span>
                                                        <br></br>
                                                        <span className="text-descriptions fw-normal">{cart.product.description}</span>
                                                        <br></br>
                                                        <span className="text-descriptions fw-normal">Qty:</span>
                                                        <span className="text-descriptions fw-normal">{cart.quntity}</span>
                                                        <br></br>
                                                        <span className="text-descriptions fw-normal">Price:</span>
                                                        <span className="text-descriptions fw-normal">{cart.unitPrice.toFixed(2)}</span>
                                                        <br></br>
                                                        <span className="text-descriptions fw-normal" > Total Price :</span>
                                                        <span className="text-descriptions fw-bold" style={{ float: "right" }} >{cart.totalPrice.toFixed(2)}</span>

                                                    </div>
                                                </div>
                                            ))}
                                            <div className="row">
                                                <div className="card-footer col-md-10 ms-3">
                                                    <ul className="list-group list-group-flush">
                                                        <li className="list-group-item d-flex justify-content-between align-items-center  fw-bold text-uppercase">
                                                            Total to pay
                                                            <span>{totalPrice.toFixed(2)}</span>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>


                                        </div>
                                        : null}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className=" position-static">
                                <div className="card">
                                    <div onClick={onClickPayment} className="card-header py-1" style={{ backgroundColor: "gray" }} >
                                        <h5 className="mb-0 text-font" style={{ color: "white" }} >Payment Method  </h5>
                                    </div>
                                    {showResultsPayment ?
                                        <PaymentOptions sendDataToParent={sendDataToParent} />
                                        : null}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className=" position-static">
                                <div className="card" style={{ border :"none", paddingTop:"15px"}} >
                                    <button type="submit" className="btn btn-success btn-yellow btn-right" style={{ width:"15%" }} onClick={() => handleOrderSubmit()} >Proceed to Buy</button>
                                </div>
                            </div>
                        </div>
                    </section>

                </div>
            </AuthenticatedTemplate>
            <UnauthenticatedTemplate>
                <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
            </UnauthenticatedTemplate>
        </>
    )
}
export default OrderCreation;