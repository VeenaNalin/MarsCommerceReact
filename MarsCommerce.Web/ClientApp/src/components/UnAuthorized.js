import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../authConfig';
import React from 'react';
function UnAuthorized(props) {
    const { instance } = useMsal();
    const handleLoginPopup = () => {
        instance
            .loginPopup({
                ...loginRequest,
                redirectUri: '',
            })
            .catch((error) => console.log(error));
    };

    return (
     
        <section className="h-100 gradient-custom"  >
            <div> <br></br></div>
            <div className="container py-5 h-900">
                <div className="row d-flex justify-content-center align-items-center h-100">
                    <div className="col-lg-10 col-xl-4">
                        <img style={{ width: 350, height: 280, }} src="./UnAuthorize.png" ></img>
                      <div><br></br></div>
                        <h3>
                            {props.data1}</h3>
                        <h6>
                            {props.data2}</h6>
                        <div><br></br></div>
                        <div className=" d-flex justify-content-center align-items-center h-100" >
                            <button className="btn btn-success"  onClick={handleLoginPopup} style={{ backgroundColor: "orange", color: "black" }}  >Login</button>
                        </div>
                    </div>
                </div>
            </div>

        </section >

    )
}
export default UnAuthorized;