import AddAddress from "./components/AddAddress";
import Addresses from "./components/Addresses";
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import ListAddress from "./components/ListAddress";
import OrderCreation from "./components/Order/OrderCreation";
import OrderDetails from "./components/Order/OrderDetails";
import OrderSummary from "./components/Order/OrderSummary";
import { Payment } from "./components/Payment";
import AddProduct from "./components/Product/AddProduct";
import PaymentOptions from "./components/Product/PaymentOptions";
import ProductDetails from "./components/ProductDetails";
import ProductListing from "./components/ProductListing";
import ShoppingCart from "./components/ShoppingCart";

const AppRoutes = [
    { index: true, element: <ProductListing /> },
    { path: '/counter', element: <Counter /> },
    { path: '/fetch-data', element: <FetchData /> },
    { path: '/cart', element: <ShoppingCart /> },
    { path: '/payment/GetPaymentInfo', element: <Payment /> },
    { path: '/ProductDetails', element: <ProductDetails /> },
    { path: '/addproduct', element: <AddProduct /> },
    { path: '/Address', element: <AddAddress /> },
    { path: '/listAddress', element: <ListAddress /> },
    { path: '/addresses', element: <Addresses /> },
    { path: '/Order', element: <OrderCreation /> },
    { path: '/OrderSummary', element: <OrderSummary /> },
    { path: '/OrderDetails', element: <OrderDetails /> },
    { path: '/PaymentOptions', element: <PaymentOptions /> },
];
export default AppRoutes;