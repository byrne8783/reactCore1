
const merge = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = (env = {}, argv = {}) => {
    const config = {
        mode: 'development',               // "development" will get us the DefinePlugin as a matter of course
        // Turn off performance hints during development because we don't do any splitting or minification in interest of speed. 
        performance: {
            hints: false
        },
        devtool: 'inline-source-map'
    };
    return merge(common,config);
};
