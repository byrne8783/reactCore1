
const merge = require('webpack-merge');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const common = require('./webpack.common.js');

module.exports = (env = {}, argv = {}) => {
    const config = {
        mode: 'production',               // "development" will get us the DefinePlugin as a matter of course.  Wonder what happens with production
        optimization: {
            minimizer: [new UglifyJsPlugin({
                chunkFilter: (chunk) => {
                    // Exclude uglification for the `vendor` chunk
                    if (chunk.name === 'vendor') {
                        return false;
                    }
                    return true;
                }
            })]
        },
        devtool: 'source-map'
    };
    return merge(common,config);
};
