# PowerBIFiddler
A Fiddler extension that allows you to see Power BI tile data in plain text.

By default, Power BI sends tile data compressed and base64 encoded. It is difficult to debug - especially when you have a real-time dashboard and data is streaming in very quickly.  Up until now you'd have to take that data and manually decode and decompress it.  With this Fiddler extension you can now see the tile data in plain text.

The encoded and compressed version looks something like this:
```
tileDataBinaryBase64Encoded=H4sIAAAAAAAEAE2OPQuDQAyG/0tmh2tLl1t1qLRCQehSOoS7gAfnKbk4iPjfG6UFyfR+POFdwFN2HEYZGOwCLUVyAva9wD0kD/ZcwAvjRGChMbB+1gJ6EvQoqH1VPu9gpUbb4Uh5h2tFoWoNFPDk0CPPt0CM7Lr5kDdbXqcsmNwPLDG6KaKEIR0+7cX/jpO5mstj26KnfC6HfowkmglPpO76BbPzFb3XAAAA
```

The new and improved decoded and decompressed version looks something like this:

```
tileData={"descriptor":{"Select":[{"Kind":2,"Value":"M0"}]},"metadata":{},"dsr":{"DataShapes":[{"Id":"DS0","PrimaryHierarchy":[{"Id":"DM0","Instances":[{"Calculations":[{"Id":"M0","Value":"10503L"}]}]}],"IsComplete":true}]}}
```

To use, just copy the ```PowerBIFiddler.dll``` and ```Newtonsoft.Json.dll``` from the [latest PowerBIFiddler release](https://github.com/jonbgallant/PowerBIFiddler/releases) to this directory: ```C:\Users\\{user}\Documents\Fiddler2\Inspectors``` and relaunch fiddler.

You will then see a "Power BI" response inspector, which includes a new tileData property of the tile object.

![](/images/inspector.png)
