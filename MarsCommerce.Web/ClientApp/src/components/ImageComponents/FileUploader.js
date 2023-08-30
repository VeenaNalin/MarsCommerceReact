import React, { useState, useEffect } from 'react';
import uploadFileToBlob, { isStorageConfigured, deleteFileFromBlob } from '../ImageComponents/azure-storage-blob';
import './FileUploader.css'
import { v4 as uuidv4 } from 'uuid';
import DisplayImage from "../ImageComponents/DisplayImage";

const storageConfigured = isStorageConfigured();

const FileUploader = (props) => {

    // current file to upload into container
    const [fileSelected, setFileSelected] = useState();
    const [fileUploaded, setFileUploaded] = useState('');

    // UI/form management
    const [uploading, setUploading] = useState(false);
    const [inputKey, setInputKey] = useState(Math.random().toString(36));

    const [isImageRemoved, setIsImageRemoved] = useState(false);

    const onFileChange = (event) => {
        // capture file into state
        setFileSelected(event.target.files[0]);
    };

    useEffect(() => {
        props.setImageUrl(fileUploaded)
    }, [fileUploaded]);

    const onFileUpload = async () => {

        if (fileSelected && fileSelected?.name) {
            // prepare UI
            setUploading(true);
            //To create UUID for each file name 
            var newFileName = createUUIDForFile(fileSelected.name);

            // *** UPLOAD TO AZURE STORAGE ***
            await uploadFileToBlob(fileSelected, newFileName);
            setFileUploaded(newFileName);

            //// *** UPLOAD TO AZURE STORAGE ***
            //await uploadFileToBlob(fileSelected);
            //setFileUploaded(fileSelected.name);

            alert("Image Uploaded Successfully");

            setFileSelected(null);
            setUploading(false);
            setInputKey(Math.random().toString(36));
            setIsImageRemoved(false);

        }

    };

    //To create UUID for each file name 
    const createUUIDForFile = (fileName) => {
        var fileSelectedSplits = fileName.split('.');
        var extenstion = fileSelectedSplits[fileSelectedSplits.length - 1]
        var newName = uuidv4(4) + "." + extenstion;
        return newName;
    }

    //delete method to remove the existing image
    const onDelete = async () => {
        await deleteFileFromBlob(props.ImageUrl);
        setFileUploaded('');
        props.setImageUrl('');
        setIsImageRemoved(true);
    }


    //delete button in form
    const Remove = () => (
        <div>
            <label>Product Image uploaded: <span className="custom-image-name">{props.ImageUrl}</span></label>
            {/*<DisplayImage ImageUrl={props.ImageUrl} />*/}
            {props.ImageUrl && <div className="image-size"><DisplayImage ImageUrl={props.ImageUrl} /> </div> }
            <button type="button" className="btn btn-danger  custom-Remove-Button" onClick={onDelete} disabled={isImageRemoved}>
                <span className="glyphicon glyphicon-remove"></span> Remove
            </button>
        </div>

    )

    // display form
    const DisplayForm = () => (
        <div>
            <input type="file" className="custom-file-input" onChange={onFileChange} key={inputKey || ''} disabled={props.editMode && (props.ImageUrl) ? true : false} />
            <button type="submit" className="btn btn-warning btn-yellow" onClick={onFileUpload} disabled={props.editMode && (props.ImageUrl) ? true : false}>
                Upload
            </button>
        </div>
    )

    return (
        <div>
            <div className="custom-div-remove">
                {props.editMode && Remove()}
            </div>

            <label className="custom-Header"> Please upload an image of the product</label>
            {storageConfigured && !uploading && DisplayForm()}
            {storageConfigured && uploading && <div>Uploading</div>}
            <hr />
            {!storageConfigured && <div>Storage is not configured.</div>}
        </div>
    );
};

export default FileUploader;


