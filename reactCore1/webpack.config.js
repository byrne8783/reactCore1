const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    mode: 'development',                            // "production" | "development" | "whatever"
    entry: './wwwroot/js/app.js',                   // Here is where the application starts executing and webpack starts bundling
    output: {
      path: path.resolve(__dirname, 'wwwroot/dist'),// the target directory for all output files
      filename: 'bundle.js'                         // the filename template for entry chunks
    },
    plugins: [
        new MiniCssExtractPlugin({
            // Options similar to the same options in webpackOptions.output
            // both options are optional.  somewhere in here I say allStypes.css
            filename: "[name].css",
            chunkFilename: "[id].css"
        }),
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js','default']
        })

    ],
    module: {
        rules: [
            // use the two loaders stated for the css files matching the Regex specified to 'test'
            // the loaders are applied right-to-left
            { test: /\.css$/, use: [{ loader: "style-loader" }, { loader: "css-loader" }] }
            //{ test: /\.css$/, use: extractCSS.extract(['css-loader?minimize']) }

        ]
    }
};
