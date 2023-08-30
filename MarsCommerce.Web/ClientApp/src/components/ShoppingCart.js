import React, { useState, useEffect } from "react";
import { BrowserRouter as useLocation } from "react-router-dom";
import axios from 'axios';
import './ShoppingCart.css';
import DisplayImage from "./ImageComponents/DisplayImage";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import UnAuthorized from './UnAuthorized';
import secureLocalStorage from "react-secure-storage";
function ShoppingCart() {
    const [shoppingCart, setshoppingCart] = useState([]);
    const [productCount, setproductCount] = useState([]);
    const react_base_url = process.env.REACT_APP_REACT_URL;
    const api_base_url = process.env.REACT_APP_API_BASE_URL;
    const api_get_url = process.env.REACT_APP_GET_CARTITEM;
    const api_delete_url = process.env.REACT_APP_DELETE_CARTITEM;;//process.env.REACT_APP_GET_CARTITEM;
    const api_update_url = process.env.REACT_APP_UPDATE_CARTITEM;;

    const userId = secureLocalStorage.getItem("userId");

    const handleproductCount = (event, index) => {
        var k = event.target.value
        const data = {
            id: shoppingCart[index].id,
            cartId: shoppingCart[index].cartId,
            productID: shoppingCart[index].product.id,
            quntity: k,
            unitPrice: shoppingCart[index].unitPrice,
            totalPrice: shoppingCart[index].totalPrice

            //isDefaultAddress: JSON.parse(isdefaultaddress)

        };
        axios.post(api_base_url + "/" + api_update_url, data)
            .then((response) => {
                updatecartafterDelete();
            })
    }

    const UpdateCart = (count, index) => {
        shoppingCart[index].quntity = count;
    }

    const DeleteData = (index) => {
        const data = {
            id: shoppingCart[index].id,
            cartId: shoppingCart[index].cartId,
            productID: shoppingCart[index].product.id,
            quntity: shoppingCart[index].quntity,
            unitPrice: shoppingCart[index].unitPrice,
            totalPrice: shoppingCart[index].totalPrice

            //isDefaultAddress: JSON.parse(isdefaultaddress)

        };



        axios.post(api_base_url + "/" + api_delete_url, data)
            .then((response) => {
                alert('removed Successfully');
                window.location.href = '/cart';
            })

    }
    const updatecartafterDelete = () => {
		axios.get(api_base_url + api_get_url + userId)
            .then(res => res)
            .then(
                (result) => {

                    const cartItem = result;
                    setshoppingCart(cartItem.data.items);
                    setproductCount(result.data.items.length);

                }
            );
    }

    useEffect(() => {
        axios.get(api_base_url + api_get_url + userId)
            .then(res => res)
            .then(
                (result) => {
                    const cartItem = result;
                    setshoppingCart(cartItem.data.items);
                    setproductCount(result.data.userId > 0 ? result.data.items.length : 0); 
                }
            );

    }, []);

    return (
        <>
            <AuthenticatedTemplate>
                <div className="container">

                    <table id="cart" className="table table-hover table-condensed">
                        <thead>
                            <tr>
                                <th style={{ width: "50%" }} >Product</th>
                                <th style={{ width: "10%" }} >Price</th>
                                <th style={{ width: "8%" }}>Quantity</th>
                                <th style={{ width: "22%" }} className="text-center">Subtotal</th>
                                <th style={{ width: "10%" }}></th>
                            </tr>

                        </thead>
                        {<tbody>

                            {
                                shoppingCart?.map((cart, index) => (

                                    <tr key={index}>
                                        <td data-th="Product">
                                            <div className="row">
                                                {/*										Hard coded value of image 
*/}											<div className="image-size">
                                                    <DisplayImage ImageUrl={cart.product.imageUrl} />
                                                </div>
                                                <div className="col-sm-10">
                                                    <h4 className="ProductName" >{cart.product.name}</h4>
                                                    <p className="ProductDescription">{cart.product.description}</p>
                                                </div>
                                            </div>
                                        </td>
                                        <td data-th="Price">{cart.product.price}</td>
                                        <td data-th="Quantity">
                                            <input type="number" onChange={(event) => handleproductCount(event, index)} className="form-control text-center" defaultValue={cart.quntity} />
                                        </td>
                                        <td data-th="Subtotal" className="text-center">{cart.totalPrice}</td>

                                        <td className="actions" data-th="">
                                            <button type="button" className="btn btn-danger" onClick={() => DeleteData(index)} id="RemovButton">
                                                <span className="glyphicon glyphicon-remove" ></span> Remove
                                            </button>
                                        </td>
                                    </tr>

                                ))}


                        </tbody>}
                        {<tfoot>

                            <tr>
                                <td><a href="#" className="btn btn-warning"><i className="fa fa-angle-left"></i> Continue Shopping</a></td>
                                <td className="hidden-xs"></td>
                                <td></td>
                                <td></td>
                                {productCount > 0 ? <td><a type="Submit" onClick={(e) => { e.preventDefault(); window.location.href = react_base_url + 'Order' }} className="btn btn-success btn-block">Checkout <i className="fa fa-angle-right"></i></a></td>
                                    : null}
                            </tr>
                        </tfoot>}
                    </table>
                </div>
            </AuthenticatedTemplate >
            <UnauthenticatedTemplate>
                <UnAuthorized data1={'Missing Cart items?'} data2={'Login to see the items you added previously'} />
            </UnauthenticatedTemplate>
        </>
    )

}
export default ShoppingCart;