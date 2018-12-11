const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

//module.exports = {
module.exports = (env = {}, argv = {}) => {
    const config = {
        mode: argv.mode || 'development',               // "production" | "development" | "whatever"
        entry: {
            main: './wwwroot/js/app.js'                // Here is where the application starts executing and webpack starts bundling
        },
        output: {
            path: path.resolve(__dirname, 'wwwroot/dist'),// the target directory for all output files. Must be an absoute (and not relative) path
            filename: 'bundle.js',                        // the filename template for entry chunks
            publicPath : 'lib/'
        },
        plugins: [
            new MiniCssExtractPlugin({
                // Options similar to the same options in webpackOptions.output
                // both options are optional.  somewhere in here I say AllStypes.css
                filename: "AllStyles.css"
                //    ,chunkFilename: "[id].css"
            }),
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery',
                'window.jQuery': 'jquery',
                Popper: ['popper.js', 'default']
            })
        ],
        module: {
            rules: [
                // use the two loaders stated for the css files matching the Regex specified to 'test'
                // the loaders are applied right-to-left
                //{ test: /\.css$/, use: [{ loader: "style-loader" }, { loader: "css-loader" }] }
                { test: /\.css$/, use: [{ loader: MiniCssExtractPlugin.loader }, { loader: 'css-loader' }] }
            ]
        }
    };
    return config;
};
