import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import axios from "axios";
import React, { useEffect, useRef, useState } from 'react';
import { useLocation } from "react-router-dom";
import DisplayImage from "../ImageComponents/DisplayImage";
import FileUploader from "../ImageComponents/FileUploader";
import UnAuthorized from '../UnAuthorized';
import { useMsal } from '@azure/msal-react'; 

function AddProduct() {
    const baseURL = process.env.REACT_APP_BASE_URL;
    const [arr, setArr] = useState([]);
    const [attrData, setattrData] = useState([]);
    const [Name, setName] = useState('');
    const [Description, setDescription] = useState('');
    const [StockCount, setStockCount] = useState('');
    const [Price, setPrice] = useState('');
    const [ImageUrl, setImageUrl] = useState('');
    const [StockKeepingUnit, setStockKeepingUnit] = useState('');
    const [Attributes, setAttributes] = useState([]);
    const [ProductAttributesData, setProductAttributesData] = useState([]);
    const [CategoriesData, setCategoriesData] = useState([]);
    const [selectedCat, setselectedCat] = useState('Choose...');
    const [selectedAttr, setselectedAttr] = useState('Choose...');
    const [selectedAttrVal, setselectedAttrVal] = useState('');
    const [selectedAttrName, setselectedAttrName] = useState('');
    const reactBaseURL = process.env.REACT_APP_REACT_URL;
    const attrComboRef = useRef(null);
    const { instance, inProgress } = useMsal();
    let activeAccount;
    if (instance) {
        activeAccount = instance.getActiveAccount();
       
    }

     

    //Edit mode
    const { search } = useLocation();
    const productId = new URLSearchParams(search).get("id");
    const editMode = productId > 0 ? true : false;

    const handleName = (event) => setName(event.target.value)
    const handleDescription = (event) => setDescription(event.target.value)
    const handleStockCount = (event) => {
        const value = event.target.value.replace(/\D/g, '').slice(0, 5);
        setStockCount(value);
    };
    const handlePrice = (event) => {
        const value = event.target.value.replace(/\D/g, '').slice(0, 10);
        setPrice(value);
    };
    const handleStockKeepingUnit = (event) => setStockKeepingUnit(event.target.value)
    const handleselectedCat = (event) => setselectedCat(event.target.value)
    const handleselectedAttrVal = (event) => setselectedAttrVal(event.target.value)

    const handleselectedAttr = (event) => {
        var index = event.nativeEvent.target.selectedIndex;
        setselectedAttr(event.target.value);
        setselectedAttrName(event.nativeEvent.target[index].text);
    }

    const AddItem = (newItem) => {
        if (editMode) {
            setattrData(prev => [...prev, newItem]);
            ClearAttrInputs();
        } else {
            if (attrComboRef.current.selectedIndex > 0) {
                setattrData(prev => [...prev, newItem]);
                ClearAttrInputs();
            }
            else {
                alert('The attribute should not be blank')
            }
        }

    }

    const RemoveItem = (id) => {
        setattrData(prev => prev.filter(item => item.ProductAttributeId !== id));
    }

    //Get Form load data
    useEffect(() => {
        LoadFormData();
    }, []);


    const ClearFormData = () => {
        setArr([]);
        setName('');
        setDescription('');
        setStockCount('');
        setPrice('');
        setImageUrl('');
        setStockKeepingUnit('');
        setAttributes([]);
        setProductAttributesData([]);
        setCategoriesData([]);
        setselectedCat('');
        setselectedAttr('');
        setselectedAttrVal('');
        setselectedAttrName('');
        setattrData([]);
        alert(editMode ? "Product updated" : "Product saved");
        window.location.href = reactBaseURL;
    }

    const ClearAttrInputs = () => {
        setselectedAttr('Choose...');
        setselectedAttrVal('');
        setselectedAttrName('');
        if (!editMode) { attrComboRef.current.selectedIndex = 0; }

    }

    const FormValidated = () => {
        if (parseInt(selectedCat) > 0) {
            return true
        } else {
            return false
        }
    }

    const LoadFormData = () => {
        try {
      
            
            //get attributes
            //const config = {
                
            //    headers: {
            //        Authorization: 'Bearer ' + localStorage.getItem('accessToken')
            //    }
            //};
            axios.get(baseURL + process.env.REACT_APP_GET_ATTRIBUTES_API).then((response) => {
                setProductAttributesData(response.data);
            }, []);

            //get categories
            axios.get(baseURL + process.env.REACT_APP_GET_CATEGORIES_API).then((response) => {
                setCategoriesData(response.data);
            }, []);

            if (editMode) {
                //get product data
                axios.get(baseURL + process.env.REACT_APP_GET_PRODUCT_DETAILS_BY_ID_API + productId).then((response) => {
                    SetFormData(response.data);
                }, []);
            }
        } catch (error) {
        }
    }

    const SaveProduct = () => {
        const data = {
            Name: Name,
            Description: Description,
            StockCount: StockCount,
            Price: Price,
            ImageUrl: ImageUrl,
            StockKeepingUnit: StockKeepingUnit,
            CategoryId: selectedCat,
            Attributes: attrData
        };

        try {
            axios.post(baseURL + process.env.REACT_APP_ADD_PRODUCT_API, data)
                .then((response) => {
                    ClearFormData();
                })
                .catch(error => {
                    console.error(error);
                });
        } catch (error) {
        }
    }
    const SetFormData = (data) => {

        setName(data.name);
        setDescription(data.description);
        setStockCount(data.stockCount);
        setPrice(data.price);
        setImageUrl(data.imageUrl);
        setStockKeepingUnit(data.stockKeepingUnit);
        setselectedCat(data.category.id);
        setattrData([]);
        const sample = data.attributes.map(buildUpdateAttr);
    }

    const buildUpdateAttr = (rw) => {
        var temp = {
            id: rw.id,
            productId: rw.productId,
            ProductAttributeId: rw.productAttributeId,
            ProductAttributeValue: rw.productAttributeValue,
            ProductAttributeName: rw.productAttribute.name
        }
        AddItem(temp);
    }

    const UpdateProduct = () => {
        try {
            const data = {
                id: productId,
                name: Name,
                description: Description,
                stockCount: StockCount,
                price: Price,
                imageUrl: ImageUrl,
                stockKeepingUnit: StockKeepingUnit,
                categoryId: selectedCat,
                Attributes: attrData
            };
            axios.post(baseURL + process.env.REACT_APP_UPDATE_PRODUCT_API, data)
                .then((response) => {
                    ClearFormData();
                })
                .catch(error => {
                    console.error(error);
                });
        } catch (error) {
        }
    }

    return (
        <>
            <AuthenticatedTemplate>

                <form>
                    <h3><b>Product | {editMode ? "Update Product" : "Add Product"}</b></h3>
                    <hr></hr>
                    <div className="row">
                        <div className="form-group col-md-6">
                            <label >Product Name</label>
                            <input value={Name} onChange={handleName} type="text" className="form-control" required />
                        </div>
                        <div className="form-group col-md-6">

                            <label >Product Category</label>
                            <select value={selectedCat} onLoad={handleselectedCat} onChange={handleselectedCat} onSelect={handleselectedCat} className="form-select" required>
                                <option disabled>Choose...</option>
                                {CategoriesData?.map((cat) =>
                                    <option value={cat.id} key={cat.id}>{cat.name}</option>
                                )}
                            </select>
                        </div>
                    </div>
                    <div className="row">
                        <div className="form-group col-md-6">
                            <label >Product Description</label>
                            <input value={Description} onChange={handleDescription} type="text" id="Description" name="Description" className="form-control" required />
                        </div>
                        <div className="form-group col-md-6">

                            <label>Stock Keeping Unit</label>
                            <input type="text" value={StockKeepingUnit} onChange={handleStockKeepingUnit} id="StockKeepingUnit" name="StockKeepingUnit" className="form-control" required />
                        </div>
                    </div>
                    <div className="row">
                        <div className="form-group col-md-6">
                            <label>Stock Count</label>
                            <input type="number" style={{ textAlign: "right" }} value={StockCount} onChange={handleStockCount} onKeyDown={(evt) => evt.key === 'e' && evt.preventDefault()} id="StockCount" name="StockCount" className="form-control" maxLength="5" required />
                        </div>
                        <div className="form-group col-md-6">

                            <label>Price</label>
                            <input inputmode="numeric" style={{ textAlign: "right" }} value={Price} onChange={handlePrice} id="Price" name="Price" className="form-control" maxLength="10" required />
                        </div>
                    </div>
                    <div className="row">
                        <div className="form-group col-md-6">
                            <label>Product Attribute</label>
                            <select value={selectedAttr} ref={attrComboRef} onLoad={handleselectedAttr} onChange={handleselectedAttr} onSelect={handleselectedAttr} id="ProductCategory" name="ProductAttribute,handleselectedAttrName" className="form-control col-lg-2" >
                                <option disabled>Choose...</option>
                                {ProductAttributesData?.map((attr) =>
                                    <option key={attr.id} tag={attr.name} value={attr.id} >{attr.name}</option>
                                )}
                            </select>
                        </div>

                        <div className="form-group col-md-5">
                            <label>Value</label>
                            <input value={selectedAttrVal} onChange={handleselectedAttrVal} type="text" id="AttrValue" name="AttrValue" className="form-control" />                            
                        </div>
                        <div className="form-group col-md-1" style={{paddingLeft:"1px"}} >
                            <br/>
                            <input type="button" onClick={() => AddItem({ ProductAttributeId: selectedAttr, ProductAttributeValue: selectedAttrVal, ProductAttributeName: selectedAttrName })} className="btn btn-warning btn-yellow" value="+" />
                        </div>
                    </div>



                    <div className="form-group col-md-6">
                        <table><tbody>
                            {
                                attrData.map(item => (<tr key={item.ProductAttributeId} >
                                    <td>{item.ProductAttributeName} </td>
                                    <td>: {item.ProductAttributeValue}</td>
                                    <td> <input type="button" onClick={() => { RemoveItem(item.ProductAttributeId) }} className="btn btn-danger" value="-" style={{ height: "30px" }} /></td>
                                </tr>
                                ))}
                        </tbody></table>
                    </div>
                    <div className="form-group col-md-6">
                        <FileUploader setImageUrl={setImageUrl} editMode={editMode} ImageUrl={ImageUrl}></FileUploader>
                        {ImageUrl && !editMode && <div className="image-size"><span className="custom-image-name">{ImageUrl}</span> is succesfully uploaded
                            <div className="image-size"> <DisplayImage ImageUrl={ImageUrl} /> </div></div>}


                        <input type="submit" onClick={editMode ? UpdateProduct : SaveProduct} className="btn btn-warning btn-yellow" value={editMode ? "Update" : "Save"} />&nbsp;
                        <input type="button" onClick={(e) => { e.preventDefault(); window.location.href = reactBaseURL }} className="btn btn-warning btn-yellow" value="Back to home" />
                    </div>
                </form>
            </AuthenticatedTemplate>


            <UnauthenticatedTemplate>
                <UnAuthorized data1={'You dont have access'} data2={'Login with valid role'} />
            </UnauthenticatedTemplate>
        </>
    )
}
export default AddProduct;