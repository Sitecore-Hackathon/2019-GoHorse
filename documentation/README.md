# Documentation

## Summary

**Category:** Best use of xConnect and/or Universal Tracker

Users have to go through long process in order to create an account in any site.
This module enables authentication via face recognition by integrating xConnect and external face recognization API named Kairos. The image of the face is either previously captured via publicly available information (social medias, etc) or via registration. The user image and data is thus store in xDb via xConnect.

## Pre-requisites

This module depends on the following

- Sitecore 9.1 Initial Release
- Sitecore instance must be configured to use SSL/HTTPS
- A subscription to [Kairos Face Recognition](https://www.kairos.com) cloud API - you might signup for a free 14 days trial

## Installation

1. Use the Sitecore Installation wizard to install the [package](/sc.package/Face%20Login-1.0.zip)
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

Provide documentation  about your module, how do the users use your module, where are things located, what do icons mean, are there any secret shortcuts etc.

Please include screenshots where necessary. You can add images to the `./images` folder and then link to them from your documentation:

![Hackathon Logo](images/hackathon.png?raw=true "Hackathon Logo")

You can embed images of different formats too:

![Deal With It](images/deal-with-it.gif?raw=true "Deal With It")

And you can embed external images too:

![Random](https://placeimg.com/480/240/any "Random")

## Video

Please provide a video highlighing your Hackathon module submission and provide a link to the video. Either a [direct link](https://www.youtube.com/watch?v=EpNhxW4pNKk) to the video, upload it to this documentation folder or maybe upload it to Youtube...

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/EpNhxW4pNKk/0.jpg)](https://www.youtube.com/watch?v=EpNhxW4pNKk)
