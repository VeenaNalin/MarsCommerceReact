import React, { Component } from 'react';
import axios from "axios";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from './UnAuthorized';

export class AddToCart extends Component {
    static displayName = AddToCart.name;

    constructor(props)
    {
        super(props);
        this.AddItemToCart = this.AddItemToCart.bind(this);        
    }

    

    AddItemToCart()
    {
        const shopItems = [
          { ProductID: this.props.productId }
        ]
        
        const details =
            {
            UserId: this.props.userId,
            Items: shopItems
            }; 

        //let baseURL = "https://localhost:7234/AddToCart/Cart"
        const apibaseURL = process.env.REACT_APP_API_BASE_URL +"/"+ process.env.REACT_APP_ADD_ITEM_TO_CART;
        axios.post(apibaseURL, details)
            .then((response) => {
                const cartID = response.data.items[0].cartId;
                window.location.href = '/cart';

                //window.location.href = '/cart?cartId=' + cartID;
                alert("item added to cart");
            }).catch(error => {
                console.log(error);
            });

    };


    render() {
        return (
            <>
                <AuthenticatedTemplate>
            <div className="cart">
                <div className="row no-gutters">
                            <button type="submit" className="btn btn-success btn-right" style={{ backgroundColor: "orange" ,color:"black", width:'30%' }} onClick={this.AddItemToCart} id="btn_cart">Add to cart</button>
                </div>
                    </div>
                </AuthenticatedTemplate >
                <UnauthenticatedTemplate>
                    <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
                </UnauthenticatedTemplate>
            </>
        );
    }
}
