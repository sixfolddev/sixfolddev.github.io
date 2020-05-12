<template>
    <div id="inboxnavpane" data-cy="navpane">
        <nav>
        <v-navigation-drawer v-model="drawer"
        dark app class="grey darken-3 py-0">
            <v-container>
                <v-row>
                    <v-col md="9">
                        <v-icon right class="options"></v-icon> <!-- Add 'New' button for future implementation of new message creation from within inbox-->
                    </v-col>
                </v-row>
            </v-container>
            <v-list nav>
                <v-list-item data-cy="messages" router to="/inbox/messages">
                    <v-list-item-action>
                        <v-icon small></v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title class="text-left">Messages</v-list-item-title>
                    </v-list-item-content>
                    <v-chip
                        v-if="messagesUnread > 0"
                        color="green accent-3"
                        small>{{messagesUnread}}
                    </v-chip>
                </v-list-item>
                <v-list-item data-cy="invitations" router to="/inbox/invitations">
                    <v-list-item-action>
                        <v-icon small></v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title class="text-left">Invitations</v-list-item-title>
                    </v-list-item-content>
                    <v-chip
                        v-if="invitationsUnread > 0"
                        color="green accent-3"
                        small>{{invitationsUnread}}
                    </v-chip>
                </v-list-item>
                <v-list-item data-cy="sent" router to="/inbox/sent">
                    <v-list-item-action>
                        <v-icon small></v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title class="text-left">Sent</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
                <!-- <v-list-item router to="/inbox/trash">
                    <v-list-item-action>
                        <v-icon small>fas fa-tachometer-alt</v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title>Trash</v-list-item-title>
                    </v-list-item-content>
                </v-list-item> -->
            </v-list>
            <!-- temporary -->
            <!-- <v-layout row style="position: absolute; bottom: 0">
              <v-flex md-10>
                <v-list-item dense>
                  <v-btn id="newmessage" text class="success mx-0 mt-3" @click="newMessage">NEW MESSAGE</v-btn>
                </v-list-item>
              </v-flex>
            </v-layout> -->
        </v-navigation-drawer>
    </nav>
    </div>
</template>

<script>
export default {
  name: 'InboxNavPane',
  data: () => ({
    drawer: true,
    messagesUnread: 0,
    invitationsUnread: 0,
    userid: 0
  }),
  created () {
    // Get user's id from persisted store
    this.userid = this.$store.getters.userid
    this.getNewCount()
  },
  computed: {
    unreadCount () {
      return this.getNewCount()
    }
  },
  methods: {
    getNewCount () {
      let uri = `${this.$hostname}/api/inbox/${this.userid}/true/messages/count`
      let req = new Request(uri, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        mode: 'cors'
      })
      fetch(req)
        .then(response => {
          if (response.ok) {
            return response.json()
          } else {
            throw new Error(response.status)
          }
        })
        .then(data => {
          this.messagesUnread = data
        })
        .catch(err => {
          console.log(err)
        })
      uri = `${this.$hostname}/api/inbox/${this.userid}/false/messages/count`
      req = new Request(uri, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        mode: 'cors'
      })
      fetch(req)
        .then(response => {
          if (response.ok) {
            return response.json()
          } else {
            throw new Error(response.status)
          }
        })
        .then(data => {
          this.invitationsUnread = data
        })
        .catch(err => {
          console.log(err)
        })
    },
    newMessage () {
      this.$router.push('/inbox/message/send/general')
    }
  }
}
</script>

<style scoped>
.v-progress-circular {
    margin: 1rem;
}
</style>
