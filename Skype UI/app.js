const { createApp } = Vue;
const API_URL = 'https://localhost:7272/';

createApp({
    data() {
        return {
            isLogin: true,
            isAuthenticated: false,
            showSettings: false,
            showCreateChat: false,
            showChatSettings: false,
            showCallMenu: false,

            messageRefreshInterval: null,

            user: {
                name: '',
                username: '',
                phone: '',
                password: '',

                id: '',
                role: '',
            },

            settings: {
                bgColor: '#ffffff',
                fontSize: 16
            },

            chats: {
                chatsList: [],
                selectedChat: null
            },

            newChat: {
                name: '',
                description: '',
                usernames: ''
            },

            editChat: {
                name: '',
                description: '',
                usernames: ''
            },

            messages: {
                messagesList: [],
                newMessage: ''
            },
        }
    },
    mounted(){
        this.startAutoRefresh();
    },
    beforeUnmount() {
        this.stopAutoRefresh();
    },
    methods: {
        toggleMode() {
            this.isLogin = !this.isLogin;
        },
        toggleCallMenu() {
            this.showCallMenu = !this.showCallMenu;
        },
        startAutoRefresh() {
            this.messageRefreshInterval = setInterval(() => {
                if (this.chats.selectedChat) {
                    this.getMessageData(this.chats.selectedChat.Id);
                }
            }, 3000); // Refresh every 30 seconds
        },
        stopAutoRefresh() {
            if (this.messageRefreshInterval) {
            clearInterval(this.messageRefreshInterval);
            this.messageRefreshInterval = null;
            }
        },
        async parseJWT(token) {
            if (!token) return null;

            const payloadBase64Url = token.split('.')[1];
            const base64 = payloadBase64Url
                .replace(/-/g, '+')
                .replace(/_/g, '/')
                .padEnd(payloadBase64Url.length + (4 - payloadBase64Url.length % 4) % 4, '=');
            const binary = atob(base64);
            const payloadJson = decodeURIComponent(
                binary
                    .split('')
                    .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                    .join('')
            );
            const payload = JSON.parse(payloadJson);

            let role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"; // Path of Claims.Roles

            this.user.role = payload[role];
            this.user.id = payload.id;
            this.user.username = payload.username;
            this.user.name = payload.name;

        },
        async submitLoginRegisterForm() {
            switch (this.isLogin) {
                case true:
                    await this.login();
                    break;
                case false:
                    await this.register();
                    break;
            }
        },
        async loadSettings() {
            await axios.get(API_URL + `SettingData/${this.user.id}`, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            }).then(response => {
                this.settings.bgColor = response.data[0].BgColour;
                this.settings.fontSize = response.data[0].FontSize;
                this.applySettingsToCSS();
            });
        },
        async applySettings() {
            const formData = new FormData();
            formData.append("id", this.user.id);
            formData.append("bgColour", this.settings.bgColor);
            formData.append("fontSize", this.settings.fontSize);

            await axios.put(API_URL + "SettingData", formData, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            }).then(() => {
                this.applySettingsToCSS();
            });
        },
        applySettingsToCSS() {
            document.documentElement.style.setProperty('--bg-color', this.settings.bgColor);
            document.documentElement.style.setProperty('--font-size', this.settings.fontSize + 'px');
        },
        async login() {
            if (!this.user.username || !this.user.password) {
                alert("Заповніть всі поля!");
                return;
            }

            const formData = new FormData();
            formData.append("username", this.user.username);
            formData.append("password", this.user.password);

            await axios.post(API_URL + "UserData/authenticate", formData)
                .then(response => {
                    this.user.username = '';
                    this.user.password = '';
                    localStorage.setItem('token', response.data.token);
                    this.parseJWT(localStorage.getItem('token'));
                    this.getChatData();
                    this.loadSettings();
                    this.isAuthenticated = true;
                })
                .catch(error => {
                    alert("Невірний логін або пароль!");
                    console.error(error);
                });
        },
        async register() {
            if (!this.user.name || !this.user.username || !this.user.phone || !this.user.password) {
                alert("Заповніть всі поля!");
                return;
            }

            const formData = new FormData();
            formData.append("name", this.user.name);
            formData.append("username", this.user.username);
            formData.append("phone", this.user.phone);
            formData.append("password", this.user.password);
            formData.append("role", 1); // Default role: User

            await axios.post(API_URL + "UserData", formData)
                .then(response => {
                    this.toggleMode();
                    alert("Реєстрація успішна! Тепер ви можете увійти.");
                })
                .catch(error => {
                    console.error(error);
                    alert("Помилка реєстрації. Спробуйте ще раз.");
                });
        },
        async getChatData() {
            await axios.get(API_URL + `ChatData/${this.user.id}`, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            })
                .then(response => {
                    this.chats.chatsList = response.data;
                });
        },
        async createChat() {
            if (!this.newChat.name || !this.newChat.description || !this.newChat.usernames) {
                alert("Заповніть всі поля!");
                return;
            }

            const usernamesArray = this.newChat.usernames
                .split(',')
                .map(name => name.trim().toLowerCase())
                .filter(name => name.length > 0);

            const formData = new FormData();
            formData.append("name", this.newChat.name);
            formData.append("description", this.newChat.description);

            usernamesArray.forEach(username => {
                formData.append("usernames", username);
            });


            await axios.post(API_URL + "ChatData", formData, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            })
                .then(() => {
                    this.newChat.name = '';
                    this.newChat.description = '';
                    this.newChat.usernames = '';
                    this.showCreateChat = false;
                    this.getChatData();
                });
        },
        async editSelectedChat() {
            if (!this.editChat.name || !this.editChat.description || !this.editChat.usernames) {
                alert("Заповніть всі поля!");
                return;
            }

            const usernamesArray = this.editChat.usernames
                .split(',')
                .map(name => name.trim().toLowerCase())
                .filter(name => name.length > 0);

            const formData = new FormData();
            formData.append("id", this.chats.selectedChat.Id);
            formData.append("name", this.editChat.name);
            formData.append("description", this.editChat.description);

            usernamesArray.forEach(username => {
                formData.append("usernames", username);
            });

            await axios.put(API_URL + "ChatData", formData, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            })
                .then(() => {
                    this.chats.selectedChat.Name = this.editChat.name;
                    this.chats.selectedChat.Description = this.editChat.description;
                    this.editChat.name = '';
                    this.editChat.description = '';
                    this.editChat.usernames = '';
                    this.showChatSettings = false;
                    this.getChatData();
                });
        },
        async deleteChat() {
            if (!confirm("Ви впевнені, що хочете видалити цей чат?")) {
                return;
            }

            await axios.delete(API_URL + `ChatData/${this.chats.selectedChat.Id}`, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            });

            this.chats.selectedChat = null;
            this.messages.messagesList = [];
            this.getChatData();
        },
        async getMessageData(chatId) {
            await axios.get(API_URL + `MessageData/${chatId}`, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            })
                .then(response => {
                    this.messages.messagesList = response.data;
                });
        },
        async sendMessage() {
            if (!this.messages.newMessage) {
                return;
            }

            const formData = new FormData();
            formData.append("chatId", this.chats.selectedChat.Id);
            formData.append("ownerId", this.user.id);
            formData.append("text", this.messages.newMessage);

            await axios.post(API_URL + "MessageData", formData, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            });

            this.messages.newMessage = '';
            await this.getMessageData(this.chats.selectedChat.Id);

            this.$nextTick(() => {
                this.ScrollToBottom();
            });
        },
        async selectChat(chat) {
            this.chats.selectedChat = chat;
            await this.getMessageData(chat.Id);

            this.$nextTick(() => {
                this.ScrollToBottom();
            });
        },
        openChatSettings() {
            this.editChat.name = this.chats.selectedChat.Name;
            this.editChat.description = this.chats.selectedChat.Description;

            this.editChat.usernames = this.chats.selectedChat.Usernames.join(', ');

            this.showChatSettings = true;
        },
        formatTime(timestamp) {
            if (!timestamp.endsWith('Z')) {
                timestamp += 'Z';
            }

            const date = new Date(timestamp);
            return date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
        },
        ScrollToBottom() {
            const el = this.$refs.msgContainer;
            el.scrollTop = el.scrollHeight;
        },
        async startMeetCall() {
            const link = "https://meet.google.com/new"

            this.messages.newMessage = "Долучайтесь до Meet дзвінка: ";

            this.toggleCallMenu();

            window.open(link, '_blank');
        },
        async startJitSiCall() {
            const room = "chat_" + this.chats.selectedChat.Id + "_" + this.chats.selectedChat.Name;
            const link = `https://meet.jit.si/${room}`;

            this.messages.newMessage = "Долучайтесь до Jitsy дзвінка: " + link;
            await this.sendMessage();

            this.toggleCallMenu();

            window.open(link, '_blank');
        }
    },
    /* watch:{
        'chats.selectedChat': function () {
            this.$nextTick(() => {
                this.ScrollToBottom();
            });
        },
        'messages.messagesList': function () {
            this.$nextTick(() => {
                this.ScrollToBottom();
            });
        }
    } */
}).mount('#app');