module.exports = {
  transpileDependencies: [
    'vuetify'
  ],
  devServer: {
    open: process.platform === 'darwin',
    host: '0.0.0.0',
    port: 44393, // CHANGE YOUR PORT HERE!
    https: true,
    hotOnly: false
  },
  configureWebpack: {
    devtool: 'source-map'
  }
}