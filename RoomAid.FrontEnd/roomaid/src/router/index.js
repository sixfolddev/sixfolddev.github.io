import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
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
    name: 'SentMessages',
    component: () => import('../views/ReadMessageView')
  },
  {
    path: '/inbox/message/send/:type',
    name: 'SentMessages',
    component: () => import('../views/SendMessageView')
  } // type: general, invitation;
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
