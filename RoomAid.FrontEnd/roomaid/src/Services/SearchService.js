import axios from 'axios'

const apiClient = axios.create({
  baseURL: 'https://localhost:8080',
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*'
  }
})

export default {
  method: {

  },
  autocompleteSearch () {
    return apiClient.get('api/search/autocomplete')
  }
}
