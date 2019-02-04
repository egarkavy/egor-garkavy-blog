var path = require("path");
var webpack = require("webpack");

var NODE_ENV = process.env.NODE_ENV || "develop"; //NOT I use $env:NODE_ENV='develop' (or 'prod') then you can use just 'webpack' or 'webpack-dev-server'

var HtmlWebpackPlugin = require("html-webpack-plugin");
var CleanWebpackPlugin = require("clean-webpack-plugin");

module.exports = {
    entry:{
        polyfills: './src/polyfills.ts',
        vendor: './src/vendor.ts',
        app: './src/main.ts'        
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: '[name].[hash].bundle.js',
        pathinfo: NODE_ENV === "develop" 
        // publicPath: '/',
    },
    resolve: {
        extensions: ['.ts', '.js']
    },

    // watch: NODE_ENV === 'develop',
    // watchOptions: {
    //     aggregateTimeout: 100 //default 300
    // },

    devtool: NODE_ENV === 'develop' ? 'source-map' : undefined,

    module: {
        // noParse: (content) => { //use it if you 100% sure that file doesn't 'import' other dependencies 
        //     return /fileName/.test(content);
        //   },

        rules: [
            { 
                test: /\.scss$/,
                exclude: [path.resolve(__dirname, "src/styles")],
                use: [
                    'to-string-loader',
                    'css-loader',
                    'sass-loader'
                ]
            },
            { 
                test: /\.scss$/,
                include: [path.resolve(__dirname, "src/styles")],
                use: [
                    'style-loader',
                    'css-loader',
                    'sass-loader'
                ]
            },
            { 
                test: /\.css$/,
                exclude: [path.resolve(__dirname, "src/styles")],
                use: [
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.ts$/,
                use: [
                    'awesome-typescript-loader',
                    'angular2-template-loader'
                ]
            },
            {
                test: /\.html$/, 
                use: [
                    'html-loader'
                ]
            },
            {
                test: /\.(jpe?g|png|gif)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[hash].[ext]',
                            outputPath: 'img/',
                            publicPath: ''
                        }
                    }
                ]
            },
            {
                test: /\.(ttf|woff|woff2)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[hash].[ext]',
                            outputPath: 'fonts/',
                            publicPath: ''
                        }
                    }
                ]
            }
        ]
    },

    optimization: { //TODO: check docs when created
        minimize: false,
        splitChunks: {
            name: 'angular-chunck',
            chunks: "all"
        }
    },

    plugins: [
        // new webpack.ProvidePlugin({ //for providing special names of 3d party libs
            // $: 'jquery'
        // }),
        new CleanWebpackPlugin(['dist']),
        // new webpack.optimize.UglifyJsPlugin({ //use for prod
        //     sourceMap: true
        // }),
        new HtmlWebpackPlugin({
            template: "./src/index.html"
        }),
        new webpack.DefinePlugin({
            NODE_ENV: `'${NODE_ENV}'`
        })
    ]
};