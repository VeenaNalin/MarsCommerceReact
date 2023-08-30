import React, { useState, useEffect } from "react";
import ReactPaginate from "react-paginate";
import axios from 'axios';
import './ProductListing.css';
import DisplayImage from './ImageComponents/DisplayImage';
import {
    BrowserRouter as Router, Link,
    useLocation
} from "react-router-dom";
import { useMsal } from '@azure/msal-react';
import secureLocalStorage from "react-secure-storage";

function ProductListing() {
    const [products, setproducts] = useState([]);
    const [pageCount, setpageCount] = useState(0);
    let limit = 6;
    const api_base_url = process.env.REACT_APP_API_BASE_URL;
    const api_get_url = process.env.REACT_APP_GET_ALL_PRODUCTDETAILS;
    const react_base_url = process.env.REACT_APP_REACT_URL;
    const { instance, inProgress } = useMsal();
    let activeAccount;
    if (instance) {
        activeAccount = instance.getActiveAccount();
    }

    useEffect(() => {
        //localStorage.setItem('logindetails', activeAccount)
        axios.get(api_base_url + api_get_url + '?pageIndex=0&pageSize=0&orderBy=Name')
            .then(res => res)
            .then(
                (result) => {
                    const total = result.data.length;;
                    setpageCount(Math.ceil(total / limit));
                    const product = result.data.slice(0, limit);
                    setproducts(product);
                }
            );

    }, [limit]);

    const fetchProducts = async (currentPage) => {
        const res = await fetch(
            api_base_url + api_get_url + `/?pageIndex=${currentPage}&pageSize=${limit}&orderBy=Name`

        );
        const data = await res.json();
        return data;
    };

    const handlePageClick = async (data) => {
        let currentPage = data.selected + 1;
        const productFormServer = await fetchProducts(currentPage);
        setproducts(productFormServer);
    };




    return (
        <div>
            <div className="container mydiv"  >

                <div className="row">

                    {products.map(pro => (

                        <div key={pro.id} className="col-md-4">
                            <div className="bbb_deals">

                                {secureLocalStorage.getItem('isAdmin') === 'true' ?
                                    <Link to={react_base_url + 'AddProduct?id=' + pro.id} >
                                        <img src="./EditIcon.png" alt="Edit" style={{ width: 30, height: 30 }} />
                                    </Link>
                                    : null}

                                <div className="ribbon ribbon-top-right"></div>
                                <div className="bbb_deals_title"></div>
                                <div className="bbb_deals_slider_container">
                                    <div className=" bbb_deals_item">
                                        <div className="image-size" onClick={(e) => { e.preventDefault(); window.location.href = react_base_url + 'ProductDetails?id=' + pro.id; }}>
                                            <DisplayImage ImageUrl={pro.imageUrl} />
                                        </div>
                                        <div className="bbb_deals_content" >

                                            <div className="bbb_deals_info_line d-flex flex-row justify-content-start">
                                                <div className="bbb_deals_item_name" >{pro.name}</div>
                                            </div>
                                            <div className="bbb_deals_info_line d-flex flex-row justify-content-start">
                                                <div className="bbb_deals_item_price ml-auto"  > <span>{pro.description}</span></div>
                                            </div>
                                            <div className="bbb_deals_info_line d-flex flex-row justify-content-start">
                                                <div className="bbb_deals_item_price ml-auto"  >        Price: <span>{pro.price}</span></div>
                                            </div>
                                            <div className="available">
                                                <div className="available_line d-flex flex-row justify-content-start">

                                                    <div className="sold_stars ml-auto"> <i className="fa fa-star"></i> <i className="fa fa-star"></i> <i className="fa fa-star"></i> <i className="fa fa-star"></i> <i className="fa fa-star"></i> </div>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    ))}
                </div>
            </div>
            <ReactPaginate
                previousLabel={"previous"}
                nextLabel={"next"}
                breakLabel={"..."}
                pageCount={pageCount}
                marginPagesDisplayed={2}
                pageRangeDisplayed={4}
                onPageChange={handlePageClick}
                containerClassName={"pagination justify-content-center"}
                pageClassName={"page-item"}
                pageLinkClassName={"page-link"}
                previousClassName={"page-item"}
                previousLinkClassName={"page-link"}
                nextClassName={"page-item"}
                nextLinkClassName={"page-link"}
                breakClassName={"page-item"}
                breakLinkClassName={"page-link"}
                activeClassName={"active"}
            />
        </div>
    )
}
export default ProductListing;