<template>
<div id = "SentMessagesView"> <!-- Start inbox container -->
  <v-container fluid>
    <InboxNavPane/>
    <InboxContainer/>
  <div class="messages"> <!-- Start inbox container -->
    <!-- Start message items -->
  </div>
  </v-container>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavPane from '@/components/InboxNavPane.vue'
import InboxContainer from '@/components/InboxContainer.vue'

export default {
  name: 'SentMessagesView',
  components: {
    InboxNavPane,
    InboxContainer
  },
  data: () => ({

  }),
  methods: {

  }
}
</script>

<style scoped>

</style>
