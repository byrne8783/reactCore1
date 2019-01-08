const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const CssOptimizePlugin = require('optimize-css-assets-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = (env = {}, argv = {}) => {
    const isDevBuild = !(env && env.prod);
    let config = {
        mode: isDevBuild ? 'development' : argv.mode || 'development',               // "production" | "development" | "whatever"
        resolve: { extensions: ['*', '.ts', '.js'] },     // supposed to mean 'look in .js and .ts for modules specified without and extension;  but a bit hairy!
        entry: {
            main: './client/js/app.ts'                // Here is where the application starts executing and webpack starts bundling  
        },
        output: {
            path: path.resolve(__dirname, 'wwwroot/dist'),// the target directory for all output files. Must be an absoute (and not relative) path
            filename: 'bundle.js',                        // the filename template for entry chunks
            publicPath: 'dist/' // has implicatins for files referenced in CSS ( fonts,images) but has major implications  path.resolve(__dirname, 'client/') /dist/
            // for the functioning of WebPackDevMiddleware.  WDM will intercept requests for files in this folder  
        },
        devtool: isDevBuild ? 'inline-source-map' : 'source-map',

        optimization: isDevBuild ? { minimize: false } : {
            minimizer: [new UglifyJsPlugin({
                chunkFilter: (chunk) => {
                    // Exclude uglification for the `vendor` chunk
                    if (chunk.name === 'vendor') {
                        return false;
                    }
                    return true;
                }
                }),
                new CssOptimizePlugin({})
                ]
        },
        plugins: [
            new CopyWebpackPlugin([
                {
                    from: 'bootstrap/dist/css/bootstrap.min.*', to: 'lib/bootstrap/css/', context: 'node_modules', force: true,flatten:true
                }]),
            new MiniCssExtractPlugin({
                // Options similar to the same options in webpackOptions.output
                // both options are optional.  somewhere in here I say AllStypes.css
                filename: "siteStyles.css"
                //    ,chunkFilename: "[id].css"
            }),
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery',
                jquery: 'jquery',
                'window.jQuery': 'jquery',
                Popper: ['popper.js', 'default']
            })
        ],
        module: {
            rules: [
                // use the two loaders stated for the css files matching the Regex specified to 'test'
                // the loaders are applied right-to-left
                //{ test: /\.css$/, use: [{ loader: "style-loader" }, { loader: "css-loader" }] }
                // { loader: 'postcss-loader' },
                {
                    test: /\.css$/, use: [{ loader: MiniCssExtractPlugin.loader }
                        , { loader: 'css-loader' }]
                },
                {
                    test: /\.scss$/, use: [!isDevBuild ? { loader: MiniCssExtractPlugin.loader } : {loader: 'style-loader'}
                        , { loader: 'css-loader', options: { sourceMap: isDevBuild } }, { loader: 'sass-loader', options: { sourceMap: isDevBuild } }]
                },
                {
                    test: /\.ts$/, use: [{ loader: 'ts-loader' }]}
            ]
        }
    };
    if (isDevBuild) {
        config = {
            ...config,
            performance: {
                hints: false                          // Turn off performance hints during development because we don't do any splitting or minification in interest of speed.
            }
        };
    }
    return config;
};
