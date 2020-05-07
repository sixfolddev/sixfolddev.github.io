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
      userid: 2157 // Test id: need to grab from token authentication
    },
    messageData: {
      messageid: 0,
      messageread: false,
      messagetype: '',
      otherusername: '',
      otheruserId: 0,
      prevmessageid: 0
    }
  },
  // For retrieving the current state of a variable
  getters: {
    userid: state => {
      return state.userData.userid
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
    otheruserId: state => {
      return state.messageData.otheruserId
    },
    prevmessageid: state => {
      return state.messageData.prevmessageid
    }
  },
  // For performing synchronous updates to states
  mutations: {
    updateUserId (state, newUserId) {
      state.userData.userid = newUserId
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
      state.messageData.otheruserId = type
    },
    updatePrevMessageId (state, type) {
      state.messageData.prevmessageid = type
    }
  },
  // For performing asynchronous methods
  actions: {
    updateUserId ({ commit }, newUserId) {
      commit('updateUserId', newUserId)
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
      commit('updateotherUserId', newOtherUserId)
    },
    updatePrevMessageId ({ commit }, newPrevMessageId) {
      commit('updatePrevMessageId', newPrevMessageId)
    }
  }
})

export default store
