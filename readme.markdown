A Fluent JSON library for building Knockout view models (or any other JSON object) built on top of JSON.NET. 

You can create JSON in a Razor view like this (note the Knockout extension methods):

        @JsonObject.Create()
            .AddProperty("name", "value")
            .AddProperty("childObject", c => {
                .AddProperty("childProperty", "value2")
            })
            .AddObservable("knockoutProperty", 123)

This would produce JSON similar to this:

    {"name":"value","childObject":{"childProperty":"value2"},"knockoutProperty":ko.observable(123)}
 
The Knockout methods are added via extension methods and other things can easily extend as well.  Please feel free to comment / criticize.