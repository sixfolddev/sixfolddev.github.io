import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    currentUser: {
      userid: ''
    }
  },
  mutations: {
    updateCurrentUser (state, newUserId) {
      state.currentUser.userid = newUserId
    }
  },
  actions: {
    updateCurrentUser ({ commit }, newUserId) {
      commit('updateCurrentUser', newUserId)
    }
  },
  modules: {
  },
  getters: {
    userid: state => {
      return state.userid
    }
  }
})
