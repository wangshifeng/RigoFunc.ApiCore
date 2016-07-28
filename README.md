# RigoFunc.ApiCore

This repo contains core infrastructures for Asp.Net Core, such as Exception handling, Result transformation and OAuth and so on.

[![Join the chat at https://gitter.im/xyting/RigoFunc.ApiCore](https://badges.gitter.im/xyting/RigoFunc.ApiCore.svg)](https://gitter.im/xyting/RigoFunc.ApiCore?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

# How to use

## Install *RigoFunc.ApiCore* package

To install RigoFunc.ApiCore, run the following command in the Package Manager Console

`PM> Install-Package RigoFunc.ApiCore`

## Configure services

```C# 
public void ConfigureServices(IServiceCollection services) {
    // user Api Core
    services.AddCoreWithOAuth();
}
```

## Configure middleware

```C#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    loggerFactory.AddDebug();

    app.UseMvcWithOAuth(new OAuthOptions {
        HostUrl = Configuration["oauth:hostUrl"],
        ScopeName = Configuration["oauth:scopeName"],
        ScopeSecret = Configuration["oauth:scopeSecret"]
    });
}
```


