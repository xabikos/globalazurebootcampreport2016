var path = require('path');
var webpack = require('webpack');
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var autoprefixer = require('autoprefixer');

var pkg = require('./package.json');

var ROOT_PATH = path.resolve(__dirname, 'wwwroot');

module.exports = {
  entry: {
    app: './app/index.js',
    vendor: Object.keys(pkg.dependencies)
  },
  output: {
    path: path.resolve(ROOT_PATH, 'js'),
    filename: '[name].min.js'
  },
  resolve: {
    extensions: ['', '.js', '.jsx']
  },
  module: {
    //preLoaders: [
    //  {
    //    test: /\.jsx?$/,
    //    loader:'eslint',
    //    exclude: /node_modules/
    //  }
    //],
    loaders: [
      {
        test: /\.jsx?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
      },
      {
        test: /\.css/,
        loader: ExtractTextPlugin.extract('style', 'css!postcss'),
        exclude: /node_modules/
      },
      {
        test: /\.(png|jpg|jpeg|gif|svg|woff|woff2)$/,
        loader: "url?limit=10000"
      }
    ]
  },
  plugins: [
    new ExtractTextPlugin('../css/[name].min.css'),
    new webpack.optimize.UglifyJsPlugin({
      compress: {
        warnings: false,
        screw_ie8: true
      }
    }),
    new webpack.optimize.CommonsChunkPlugin(
      'vendor',
      '[name].min.js'
    ),
    new webpack.DefinePlugin({
      // This affects react lib size
      'process.env': {
        'NODE_ENV': JSON.stringify('production')
      }
    }),
    new webpack.optimize.OccurenceOrderPlugin(),
    new webpack.optimize.DedupePlugin()
  ],
  postcss: function () {
    return [autoprefixer];
  }
};
