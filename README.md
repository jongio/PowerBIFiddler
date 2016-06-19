# PowerBIFiddler
PowerBIFiddler is a [Fiddler](http://www.telerik.com/fiddler) inspector extension that allows you to see Power BI tile data in plain text.

By default, Power BI sends tile data compressed and base64 encoded. It is difficult to debug - especially when you have a real-time dashboard and data is quicking streaming in.  Without PowerBIFiddler, you'd have to take that data and manually decode and decompress it. This extension automatically decodes and decompresses it for you and displays it in a new Fiddler Inspector response tab.

The encoded and compressed version looks something like this:
```
tileDataBinaryBase64Encoded=H4sIAAAAAAAEAE2OPQuDQAyG/0tmh2tLl1t1qLRCQehSOoS7gAfnKbk4iPjfG6UFyfR+POFdwFN2HEYZGOwCLUVyAva9wD0kD/ZcwAvjRGChMbB+1gJ6EvQoqH1VPu9gpUbb4Uh5h2tFoWoNFPDk0CPPt0CM7Lr5kDdbXqcsmNwPLDG6KaKEIR0+7cX/jpO5mstj26KnfC6HfowkmglPpO76BbPzFb3XAAAA
```

The new and improved decoded and decompressed version looks something like this:

```
tileData={"descriptor":{"Select":[{"Kind":2,"Value":"M0"}]},"metadata":{},"dsr":{"DataShapes":[{"Id":"DS0","PrimaryHierarchy":[{"Id":"DM0","Instances":[{"Calculations":[{"Id":"M0","Value":"10503L"}]}]}],"IsComplete":true}]}}
```

To use, run the PowerBIFiddler.msi found on the [releases page](https://github.com/jonbgallant/PowerBIFiddler/releases). It will copy the ```PowerBIFiddler.dll``` and ```Newtonsoft.Json.dll``` files to our local ```C:\Users\{user}\Documents\Fiddler2\Inspectors``` directory.

When you setup a real-time dashboard, Power BI will send a "subscribe" HTTP request and will get a "tiles" data response.

![](/images/fiddlersessions.png)

When you click on the "tiles" response you will see the new tileData property in the new "Power BI" inspector tab.
![](/images/inspector.png)
