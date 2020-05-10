<template>
  <div id="messagecontainer">
      <v-layout>
        <v-flex>
          <v-app style="background-color: white;" class="ma-n4">
            <v-container>
              <template>
                <v-row>From: {{sender}}</v-row>
                <v-divider></v-divider>
                <v-layout align-start>
                  <v-row>{{content}}</v-row>
                </v-layout>
                <v-row></v-row> <!-- Spacer between message and button-->
                <v-row>
                  <v-layout justify-end>
                    <v-btn id="reply" text class="success mx-0 mt-3" @click="replyMessage">REPLY</v-btn>
                  </v-layout>
                </v-row>
              </template>
            </v-container>
          </v-app>
        </v-flex>
      </v-layout>
    </div>
</template>

<script>
export default {
  name: 'MessageContainer',
  data: () => ({
    userid: 0,
    message: null,
    messageid: 0,
    prevmessageid: 0,
    otheruserid: 0,
    messagetype: '',
    sender: '',
    content: ''
  }),
  created () {
    // Get user's id from persisted store
    this.userid = this.$store.getters.userid
    // Get message id from persisted store
    this.messageid = this.$store.getters.messageid
    // Get type of message (general or invitation) from persisted store
    this.messagetype = this.$store.getters.messagetype
    // Get message sender full name from persisted store
    this.sender = this.$store.getters.otherusername
    // Fetch message from database server
    this.readMessage(this.messagetype)
  },
  methods: {
    // GET request for messages
    readMessage (type) {
      const uri = `${this.$hostname}/api/inbox/${type}/${this.userid}/${this.messageid}`
      const req = new Request(uri, {
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
          this.message = data
          this.content = data.messageBody
          this.prevmessageid = data.prevMessageID
          this.otheruserid = data.senderID
        })
        .catch(err => {
          console.log(err)
        })
    },
    replyMessage () {
      this.$store.dispatch('updatePrevMessageId', this.prevmessageid)
      this.$store.dispatch('updateOtherUserId', this.otheruserid)
      this.$router.push(`/inbox/message/reply/${this.messagetype}`)
    }
  }
}
</script>

<style scoped>
#btn {
  position: absolute;
  bottom:0;
  right:0;
}
</style>
