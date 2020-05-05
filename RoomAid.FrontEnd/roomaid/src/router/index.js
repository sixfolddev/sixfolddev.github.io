import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: () => import('../views/Home.vue')
  },
  // {
  //   path: '/householdsearch',
  //   name: 'HouseholdSearch',
  //   component: () => import('../views/HouseholdSearchView')
  // },
  {
    path: '/inbox/messages',
    name: 'MessageInbox',
    component: () => import('../views/MessageInboxView')
  },
  {
    path: '/inbox/invitations',
    name: 'InvitationInbox',
    component: () => import('../views/InvitationInboxView')
  },
  {
    path: '/inbox/sent',
    name: 'SentMessages',
    component: () => import('../views/SentMessagesView')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
