import React, { Component } from 'react';
import './NavMenu.css';
import axios from "axios";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from './UnAuthorized';


export class Payment extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            posts: [],

            payVal: 'Cash On Delivery',
            checked: 0
        }
    }
    componentDidMount() {
        var PaymentUrl = process.env.REACT_APP_BASE_URL + process.env.REACT_APP_GET_PAYMENTDETAILS;
        axios.get(PaymentUrl).then(response => {

            this.setState({ posts: response.data })
            this.campaign_types = response.data
            var element = document.getElementById("loader");
            element.classList.add("hide-loader");
        });
    }


    handleChangeCompanyType = (event, index) => {
        let value = this.state.posts[index].paymentMethod;
        this.setState({ payVal: value, checked: index })
    };

    render() {
        return (
            <>
                <AuthenticatedTemplate>
                    <div className="container ">
                        <section>
                            <div className="row card">
                                <div className=" position-static">

                                    <div className="card-body">
                                        <div className="card-header py-1">
                                            <h5 className="mb-0 text-font">Payment Info </h5>
                                        </div>
                                        {this.state.posts.map((type, i) => (
                                            <div className="d-flex flex-row pb-3">
                                                <div className="d-flex align-items-center pe-2">
                                                    <input
                                                        key={type.id}
                                                        type="radio"
                                                        value={this.state.payVal}
                                                        checked={this.state.checked === i}
                                                        onClick={(event) => this.handleChangeCompanyType(event, i)}
                                                    />
                                                </div>
                                                <div className="rounded border d-flex w-100 p-3 align-items-center">
                                                    <p className="mb-0">
                                                        <i className="fab fa-cc-visa fa-lg text-primary pe-2"></i>
                                                        <label className='ms-1 label-brand'>
                                                            <span style={{ color: "#606060", fontSize: 25 }}>{type.paymentMethod}</span>
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
        );
    }
}
