const path = require("path");
const EsmWebpackPlugin = require("@purtuga/esm-webpack-plugin");

module.exports = {
    mode: "production",
    entry: "./wwwroot/js/speech.js",
    output: {
        path: path.resolve(__dirname, "wwwroot/js"),
        filename: "microsoft-cognitiveservices-speech-sdk-bundle.js",
        library: "Speech",
        libraryTarget: "var"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                use: {
                    loader: 'babel-loader',
                },
                exclude: '/node_modules/',
            }
        ]
    },
    plugins: [
        new EsmWebpackPlugin()
    ]
}
