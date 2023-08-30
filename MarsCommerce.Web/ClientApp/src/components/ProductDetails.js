import { useIsAuthenticated } from "@azure/msal-react";
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useLocation } from "react-router-dom";
import { AddToCart } from './AddToCart';
import DisplayImage from './ImageComponents/DisplayImage';
import './ProductDetails.css';
import secureLocalStorage from "react-secure-storage";
function ProductDetails() {

    const { search } = useLocation();
    const id = new URLSearchParams(search).get("id");
    const userId = secureLocalStorage.getItem("userId");
    const [productDetails, setproductDetails] = useState([]);
    const [productDetailsAtrribute, setproductDetailsAttribute] = useState([]);
    const api_base_url = process.env.REACT_APP_API_BASE_URL + "/" + process.env.REACT_APP_GET_PRODUCT_DETAILS_BY_ID_API + id;
    const isAuthenticated = useIsAuthenticated();

    useEffect(() => {
        axios.get(api_base_url)
            .then(res => {
                if (res != null) {

                    setproductDetails(res.data);
                    setproductDetailsAttribute(res.data.attributes);
                }
            })
            .catch(error => {
                console.log(error)
            });
    }, []);



    return (
        <>

            <div className="container">
                <div className="col-lg-8 border p-3 main-section bg-white">

                    <div className="row m-0">
                        {productDetails.imageUrl && <div className="col-lg-4 left-side-product-box pb-3" >
                            <DisplayImage ImageUrl={productDetails.imageUrl} imagevisible={true} />
                        </div>}
                        <div className="col-lg-8">
                            <div className="right-side-pro-detail border p-3 m-0">
                                <div className="row">
                                    <div className="col-lg-12">

                                        <p className="m-0 p-0"> {productDetails.name}</p>
                                        <p className="m-0 p-0">{productDetails.description}</p>
                                    </div>
                                    <div className="col-lg-12">
                                        <p className="m-0 p-0 price-pro">Rs:{productDetails.price}</p>
                                    </div>

                                    {productDetailsAtrribute.length > 0 &&
                                        <div className="col-lg-12 pt-2">
                                            <h6><b>More Details</b></h6>
                                            <hr style={{ marginTop: "2px", marginBottom: "2px" }} />
                                            {productDetailsAtrribute.map((attr, index) => (
                                                <div key={index}>
                                                    <label>{attr.productAttribute.name} : {attr.productAttributeValue}</label> <br />
                                                </div>
                                            ))}

                                        </div>
                                    }
                                    {isAuthenticated ?
                                        <div className="col-lg-12 mt-3">
                                            <div className="row">
                                                <AddToCart productId={id} userId={userId} />
                                            </div>
                                        </div> : null}
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

        </>
    )

}
export default ProductDetails;