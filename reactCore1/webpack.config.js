const path = require('path');
const webpack = require('webpack');

module.exports = {
  mode: 'development',
  entry: './wwwroot/js/app.js',
  output: {
            path: path.resolve(__dirname, 'wwwroot/dist'),
            filename: 'bundle.js'
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js','default']
        })

    ]
    , module: {
        rules: [
            { test: /\.css$/, use: [{ loader: "style-loader" }, { loader: "css-loader" }] }

        ]
    }
};
