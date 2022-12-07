call ng build --deploy-url /shopinterface/ --base-href /shopinterface/ --configuration development
robocopy .\dist\shop-interface c:\inetpub\wwwroot\ShopInterface *.* /is /it
