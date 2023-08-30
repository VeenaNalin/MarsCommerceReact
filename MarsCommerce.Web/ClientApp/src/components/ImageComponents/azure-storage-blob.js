// ./src/azure-storage-blob.ts

// <snippet_package>
import { BlobServiceClient } from "@azure/storage-blob";

const containerName = process.env.REACT_APP_AZURE_STORAGE_CONTAINER_NAME;
const sasToken = process.env.REACT_APP_AZURE_STORAGE_SAS_TOKEN;
const storageAccountName = process.env.REACT_APP_AZURE_STORAGE_RESOURCE_NAME;
const domainName = process.env.REACT_APP_AZURE_STORAGE_DOMAIN_NAME;
// </snippet_package>

// <snippet_get_client>
const uploadUrl = `https://${storageAccountName}${domainName}${sasToken}`;

// get BlobService = notice `?` is pulled out of sasToken - if created in Azure portal
const blobService = new BlobServiceClient(uploadUrl);

// get Container - full public read access
const containerClient =
    blobService.getContainerClient(containerName);
// </snippet_get_client>

// <snippet_isStorageConfigured>
// Feature flag - disable storage feature to app if not configured
export const isStorageConfigured = () => {
    return !storageAccountName || !sasToken ? false : true;
};
// </snippet_isStorageConfigured>

// <snippet_getBlobsInContainer>
// return list of blobs in container to display
export const getBlobsInContainer = async ({ ImageUrl }) => {
    const returnedBlobUrls = [];
    const blobItem = {
        url: `https://${storageAccountName}.blob.core.windows.net/${containerName}/${ImageUrl}?`,
    }
    //    // if image is public, just construct URL
    returnedBlobUrls.push(blobItem);
    //}

    return returnedBlobUrls;
};
// </snippet_getBlobsInContainer>

// <snippet_createBlobInContainer>
const createBlobInContainer = async (file, newFileName) => {

    // create blobClient for container
    const blobClient = containerClient.getBlockBlobClient(newFileName);

    // set mimetype as determined from browser with file upload control
    const options = { blobHTTPHeaders: { blobContentType: file.type } };

    // upload file
    await blobClient.uploadData(file, options);
};
// </snippet_createBlobInContainer>

// <snippet_uploadFileToBlob>
const uploadFileToBlob = async (file, newFileName) => {
    if (!file) return;

    // upload file
    await createBlobInContainer(file, newFileName);
};

const deleteBlobInContainer = async (ImageUrl) => {
    // Create blob client from container client
    const blockBlobClient = await containerClient.getBlockBlobClient(ImageUrl);

    // include: Delete the base blob and all of its snapshots.
    // only: Delete only the blob's snapshots and not the blob itself.
    const options = {
        deleteSnapshots: 'include' // or 'only' 
    }

    //delete file
    await blockBlobClient.deleteIfExists(options);

}

export const deleteFileFromBlob = async (ImageUrl) => {
    if (!ImageUrl) return;

    // delete file
    await deleteBlobInContainer(ImageUrl);
}


// </snippet_uploadFileToBlob>

export default uploadFileToBlob;