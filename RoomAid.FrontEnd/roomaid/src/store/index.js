import Vue from 'vue'
import Vuex from 'vuex'
import createPersistedState from 'vuex-persistedstate'

Vue.use(Vuex)

const store = new Vuex.Store({
  plugins: [createPersistedState({
    storage: window.sessionStorage
  })],
  // Global variables that every Vue component can reference
  state: {
    userData: {
      isauthenticated: false,
      userid: 0, // Test id: need to grab from token authentication
      authenticationtoken: null
    },
    messageData: {
      messageid: 0,
      messageread: false,
      messagetype: '',
      otherusername: '',
      otheruserid: 0,
      prevmessageid: 0
    },
    searchRequest: {},
    searchResults: [],
    currentPage: 1
  },
  // For retrieving the current state of a variable
  getters: {
    userid: state => {
      return state.userData.userid
    },
    isauthenticated: state => {
      return state.userData.isauthenticated
    },
    authenticationtoken: state => {
      return state.userData.authenticationtoken
    },
    messageid: state => {
      return state.messageData.messageid
    },
    messageread: state => {
      return state.messageData.messageread
    },
    messagetype: state => {
      return state.messageData.messagetype
    },
    otherusername: state => {
      return state.messageData.otherusername
    },
    otheruserid: state => {
      return state.messageData.otheruserid
    },
    prevmessageid: state => {
      return state.messageData.prevmessageid
    },
    searchRequest: state => {
      return state.searchRequest
    },
    searchResults: state => {
      return state.searchResults
    }
  },
  // For performing synchronous updates to states
  mutations: {
    updateUserId (state, newUserId) {
      state.userData.userid = newUserId
    },
    updateAuthenticationStatus (state, newAuthenticationStatus) {
      state.userData.isauthenticated = newAuthenticationStatus
    },
    updateAuthenticationToken (state, newToken) {
      state.userData.authenticationtoken = newToken
    },
    updateMessageId (state, newMessageId) {
      state.messageData.messageid = newMessageId
    },
    updateMessageRead (state, readState) {
      state.messageData.messageread = readState
    },
    updateMessageType (state, type) {
      state.messageData.messagetype = type
    },
    updateOtherUserName (state, type) {
      state.messageData.otherusername = type
    },
    updateOtherUserId (state, type) {
      state.messageData.otheruserid = type
    },
    updatePrevMessageId (state, type) {
      state.messageData.prevmessageid = type
    },
    updateSearchRequest (state, newRequest) {
      state.searchRequest = newRequest
    },
    updateSearchResults (state, newResults) {
      state.searchResults = newResults
    }
  },
  // For performing asynchronous methods
  actions: {
    updateUserId ({ commit }, newUserId) {
      commit('updateUserId', newUserId)
    },
    updateAuthenticationStatus ({ commit }, newAuthenticationStatus) {
      commit('updateAuthenticationStatus', newAuthenticationStatus)
    },
    updateAuthenticationToken ({ commit }, newToken) {
      commit('updateAuthenticationToken', newToken)
    },
    updateMessageId ({ commit }, newMessageId) {
      commit('updateMessageId', newMessageId)
    },
    updateMessageRead ({ commit }, readState) {
      commit('updateMessageRead', readState)
    },
    updateMessageType ({ commit }, type) {
      commit('updateMessageType', type)
    },
    updateOtherUserName ({ commit }, name) {
      commit('updateOtherUserName', name)
    },
    updateOtherUserId ({ commit }, newOtherUserId) {
      commit('updateOtherUserId', newOtherUserId)
    },
    updatePrevMessageId ({ commit }, newPrevMessageId) {
      commit('updatePrevMessageId', newPrevMessageId)
    },
    updateSearchRequest ({ commit }, newRequest) {
      commit('updateSearchRequest', newRequest)
    },
    updateSearchResults ({ commit }, newResults) {
      commit('updateSearchResults', newResults)
    }
  }
})

export default store
