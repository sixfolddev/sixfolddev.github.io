<template>
  <div id="inboxcontainer" data-cy="messagelist">
      <v-layout>
        <v-flex>
          <v-app style="background-color: white;" class="ma-n4">
            <v-container>
              <template>
                <v-data-table v-model="selected"
                :headers="headers"
                :items="messages"
                item-key="messageID"
                show-select
                sort-by="sentDate"
                no-data-text="No messages"
                @click:row="openMessage"
                class="messagelist">
                <template v-slot:default="items">
                  <div v-if="items.IsRead = true"> <!-- How so set conditional for already read messages? -->
                    <tr v-bind:style="{ color: 'blue'}"></tr>
                  </div>
                </template>
                </v-data-table>
              </template>
            </v-container>
          </v-app>
        </v-flex>
      </v-layout>
    </div>
</template>

<script>
export default {
  name: 'InboxContainer',
  data: () => ({
    userid: 0,
    messages: [],
    selected: [],
    // Headers that identify a message in an inbox
    headers: [
      { text: 'Name', sortable: true, value: 'fullName' },
      { text: 'Date', sortable: true, defaultSortBy: true, value: 'sentDate' }
    ],
    messagetype: ''
  }),
  created () {
    // Get user's id from persisted store
    this.userid = this.$store.getters.userid
    // Get type of message (general or invitation) from persisted store
    this.messagetype = this.$store.getters.messagetype
    // Fetch messages from database server
    this.getMessages(this.messagetype)
  },
  methods: {
    // GET request for messages
    getMessages (type) {
      const uri = `${this.$hostname}/api/inbox/${type}/${this.userid}`
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
          data.forEach(m => {
            this.messages.push(m)
          })
        })
        .catch(err => {
          console.log(err)
        })
    },
    openMessage (item) {
      // Update message info
      this.$store.dispatch('updateMessageId', item.messageID)
      this.$store.dispatch('updateOtherUserName', item.fullName)
      this.$store.dispatch('updateMessageRead', item.isRead)
      // Route to ReadMessageView
      this.$router.push(`/inbox/message/${item.messageID}`)
    }
  }
}
</script>

<!-- # for ids, . for classes; <span> for inline <div> to style individually-->
<style scoped>
#inboxcontainer {
    display:flex;
    position: relative;
    left: 128px;
}

::v-deep tbody tr {
  cursor: pointer;
}
</style>
