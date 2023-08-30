import React, { useState, useEffect } from 'react';
import { isStorageConfigured, getBlobsInContainer } from './azure-storage-blob';
import ReactImageMagnify from 'react-image-magnify';
const storageConfigured = isStorageConfigured();


const DisplayImage = ({ ImageUrl,imagevisible }) => {
    // all blobs in container
    const [blobList, setBlobList] = useState([]);
    let imageMaginfy = false;

    // *** Display Image in Blob Storage Container ***
    useEffect(() => {
        getBlobsInContainer({ ImageUrl }).then((list) => {
            // prepare UI for results
            setBlobList(list);
            if (imagevisible === true) {
                imageMaginfy =  imagevisible ;
            }
           
        })
    },[]);

    return (
        <div>
            

           <div>
        
            {storageConfigured && blobList.length > 0 &&
                <div>
                    <ul>
                        {blobList.map((item,index) => {
                            return (
                                <div key={index}>
                                    <div>
                                        {item.name}
                                        <br />
                                       
                                        {imagevisible ? <div className="App">

                                            <div style={{ width: "200px", height: "200px" }}>
                                                <ReactImageMagnify {...{
                                                    smallImage: {
                                                        alt: 'Wristwatch by Ted Baker London',
                                                        isFluidWidth: true,
                                                        src: item.url
                                                    },
                                                    largeImage: {
                                                        src: item.url,
                                                        width: 1200,
                                                        height: 1800
                                                    }
                                                }} />
                                            </div>
                                        </div>
                                            :<img src={item.url} alt={item.name} />
                                            }
                                        <br />
                                    </div>
                                </div>
                            );
                        })}
                    </ul>
                </div>
            }
            {!storageConfigured && <div>Storage is not configured.</div>}
                </div>
            
      
</div>
        
    );
};

export default DisplayImage;
