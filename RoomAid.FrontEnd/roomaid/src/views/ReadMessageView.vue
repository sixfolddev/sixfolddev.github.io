<template>
<div id = "ReadMessageView"> <!-- Start inbox container -->
  <v-content>
    <v-container fluid>
      <v-layout row>
        <v-flex md0><InboxNavPane/></v-flex>
        <v-flex md12><MessageContainer/></v-flex>
      </v-layout>
    </v-container>
  </v-content>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavPane from '@/components/InboxNavPane.vue'
import MessageContainer from '@/components/MessageContainer.vue'

export default {
  name: 'ReadMessageView',
  components: {
    InboxNavPane,
    MessageContainer
  },
  data: () => ({
  }),
  created () {
  },
  methods: {
  }
}
</script>
