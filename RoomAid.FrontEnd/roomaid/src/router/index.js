import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    redirect: '/login',
    name: 'Default'
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/LoginView.vue')
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('../views/Home.vue')
  },
  {
    path: '/inbox',
    redirect: '/inbox/messages',
    name: 'Inbox'
  },
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
  },
  {
    path: '/inbox/message/:id',
    name: 'ReadMessage',
    component: () => import('../views/ReadMessageView')
  },
  {
    path: '/inbox/message/reply/:type',
    name: 'ReplyMessage',
    component: () => import('../views/ReplyMessageView')
  },
  {
    path: '/search',
    name: 'HouseholdSearch',
    component: () => import('../views/HouseholdSearchView')
  },
  {
    path: '/registration',
    name: 'Registration',
    component: () => import('../views/RegistrationView')
  },
  {
    path: '/householdCreation',
    name: 'HouseholdCreation',
    component: () => import('../views/HouseholdCreationView')
  },
  {
    path: '/inbox/message/send',
    name: 'SendMessage',
    component: () => import('../views/SendMessageView')
  }, // type: general, invitation;
  {
    path: '/listing',
    name: 'HouseholdListing',
    component: () => import('../views/SearchListingView')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
