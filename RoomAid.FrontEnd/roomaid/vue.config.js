module.exports = {
  transpileDependencies: [
    'vuetify'
  ],
  devServer: {
    open: process.platform === 'darwin',
    host: '0.0.0.0',
    port: 8080, // CHANGE YOUR PORT HERE!
    // https: true,
    hotOnly: false
  },
  configureWebpack: {
    devtool: 'source-map'
  }
}
