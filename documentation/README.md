# Documentation

## Summary

**Category:** Best use of xConnect and/or Universal Tracker

Users have to go through long process in order to create an account in any site.
This module enables authentication via face recognition by integrating xConnect and external face recognization API named Kairos. The image of the face is either previously captured via publicly available information (social medias, etc) or via registration. The user image and data is thus store in xDb via xConnect.

Face Login Parts

A) Sitecore CUSTOM FIELDS and SUBMIT ACTIONS to allow:

1. Reading and writting the Avatar of the current contact
* Field Types: 
		/sitecore/system/Settings/Forms/Field Types/Basic/Contact Avatar
		/sitecore/system/Settings/Forms/Field Types/Basic/Upload Contact Avatar
* Submit Action: 
		/sitecore/system/Settings/Forms/Submit Actions/Face Login/Upload Contact Avatar

2. Subscribing and Unsubscribing from the (cloud) Face Detection program (This part is important to mitigate privacy concerns)	
* The Face Recognition works as biometric ID of the person - just as fingerprint scanners
When the user opts-out, his information is totally removed from the cloud Face API, and thus no Face Login is allowed with that face.
* Field Types: 
		/sitecore/system/Settings/Forms/Field Types/Basic/Adhere to Face Login
* Submit Action: 
		/sitecore/system/Settings/Forms/Submit Actions/Face Login/Adhere to Face Login

B) Custom Component to allow Face Login (biometric authentication)
/sitecore/layout/Renderings/Feature/Login/Face Login
1. Face Login requires a Webcam, HTTPS connection
2. Requires that user gives permission to the browser to access the webcam
3. Needs good light to capture a good image

C) Demo Content
- My Account form with Avatar Upload and Opt-in checkbox
- Demo pages under /sitecore/content/Home

## Pre-requisites

This module depends on the following

- Sitecore 9.1 Initial Release
- Sitecore instance must be configured to use SSL/HTTPS
- A subscription to [Kairos Face Recognition](https://www.kairos.com) cloud API - you might signup for a free 14 days trial

## Installation

1. Use the Sitecore Installation wizard to install the [package](/sc.package/Face%20Login-1.1.zip)
2. Update the configuration file `App_Config\Include\Foundation\Foundation.Kairos.config` with the Kairos API id and key from your subscription (see below)
3. Copy the file [FaceLoginDefineModel, 1.0.json](/installation/FaceLoginDefineModel%2C%201.0.json) to the following location: `[XConnect Installation]\App_Data\jobs\continuous\IndexWorker\App_data\Models`, `[XConnect Installation]\App_Data\jobs\continuous\ProcessingEngine\App_Data\Models` and `[XConnect Installation]\App_Data\Models`.

## Configuration

Once the module is configured, update the entries `FaceLogin.Kairos_ApplicationID` and `FaceLogin.Kairos_ApplicationKey` in the configuration file `App_Config\Include\Foundation\Foundation.Kairos.config` with the Kairos API id and key from your subscription.

```xml
<?xml version="1.0"?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig">
    <sitecore>
        <settings>
            <setting name="FaceLogin.Kairos_GalleryName" value="FaceLogin" />
            <setting name="FaceLogin.Kairos_ApplicationID" value="YOUR APPLICATION ID" />
            <setting name="FaceLogin.Kairos_ApplicationKey" value="YOUR APPLICATION KEY" />
            <setting name="FaceLogin.MinimumConfidence" value="0.60" />
            <setting name="FaceLogin.MinimumQuality" value="0.60" />
        </settings>
    </sitecore>
</configuration>
```

## Usage
Sign-in Step
Access "Account" page on your local Sitecore Instance Example :https://sc910.sc/account (make sure you use HTTPS, it's a requirement)
Fill the avaliable fields, And select the Image (This image must be a selfie)
It's important to check " I agree to use my picture for Face Login", this is a important field that will be used.
Click on the "Save action"
The Page will reload, and show up your picture

![Account Page](images/account.jpg?raw=true "facelogin")

Authentication Step 
Access "Login" page on your local Sitecore Instace Ex : https://sc910.sc/login (make sure you use HTTPS, it's a requirement)
The Browser should ask permission to acess your camera and take your picture, Click "Allow" (this step will fail if you are under http)
Click on the button "Face login" , the application will tae your picture and send you to another page in case of success

![Login Page](images/facelogin.jpg?raw=true "facelogin")

![Login Successful](images/loginsuccessful.jpg?raw=true "Successful")

## Video

[direct link](https://www.youtube.com/watch?v=7G1vXGdFyOE) to the video.

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/7G1vXGdFyOE/0.jpg)](https://www.youtube.com/watch?v=7G1vXGdFyOE)
