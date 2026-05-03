// Navbar scroll effect
window.addEventListener('scroll', () => {
    const nav = document.getElementById('mainNav');
    if (nav) nav.style.boxShadow = window.scrollY > 50 ? '0 4px 30px rgba(0,0,0,0.5)' : 'none';
});

// Auto-dismiss toasts after 4 seconds
document.addEventListener('DOMContentLoaded', () => {
    const toasts = document.querySelectorAll('.alert-toast');
    toasts.forEach(t => setTimeout(() => { t.style.opacity = '0'; t.style.transition = 'opacity 0.5s'; setTimeout(() => t.remove(), 500); }, 4000));
});

// Chatbot Logic
function toggleChatbot() {
    const container = document.getElementById('ai-chatbot-container');
    const toggleBtn = document.getElementById('chatbot-toggle-btn');
    if (container.style.display === 'none' || container.style.display === '') {
        container.style.display = 'block';
        toggleBtn.style.display = 'none';
        document.getElementById('chat-input').focus();
    } else {
        container.style.display = 'none';
        toggleBtn.style.display = 'flex';
    }
}

async function sendChatMessage() {
    const input = document.getElementById('chat-input');
    const msg = input.value.trim();
    if (!msg) return;

    input.value = '';
    const chatMessages = document.getElementById('chat-messages');

    // Add User Message
    const userDiv = document.createElement('div');
    userDiv.className = 'text-end';
    userDiv.innerHTML = `<div class="d-inline-block p-2 rounded-3 text-dark fw-bold" style="background: #ffd700; border: 1px solid #e6c200; max-width: 85%;">${escapeHtml(msg)}</div>`;
    chatMessages.appendChild(userDiv);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    // Add Loading Indicator
    const typingDiv = document.createElement('div');
    typingDiv.className = 'text-start';
    typingDiv.id = 'chat-typing-indicator';
    typingDiv.innerHTML = `<div class="d-inline-block p-2 rounded-3 text-white" style="background: rgba(255,255,255,0.1); border: 1px solid rgba(255,255,255,0.05);">...</div>`;
    chatMessages.appendChild(typingDiv);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    try {
        const response = await fetch('/api/Chatbot/Send', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ message: msg })
        });

        const data = await response.json();
        document.getElementById('chat-typing-indicator')?.remove();

        const botDiv = document.createElement('div');
        botDiv.className = 'text-start';
        let responseText = data.reply || data.error || 'Unknown error occurred';
        if (data.details) responseText += ': ' + data.details;
        botDiv.innerHTML = `<div class="d-inline-block p-2 rounded-3 text-white" style="background: rgba(255,255,255,0.1); border: 1px solid rgba(255,255,255,0.05); max-width: 85%;">${escapeHtml(responseText)}</div>`;
        chatMessages.appendChild(botDiv);
    } catch (e) {
        document.getElementById('chat-typing-indicator')?.remove();
        const errorDiv = document.createElement('div');
        errorDiv.className = 'text-start';
        errorDiv.innerHTML = `<div class="d-inline-block p-2 rounded-3 text-white" style="background: rgba(220,53,69,0.2); border: 1px solid rgba(220,53,69,0.4); max-width: 85%;">Error connecting to AI service.</div>`;
        chatMessages.appendChild(errorDiv);
    }
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

function escapeHtml(unsafe) {
    return unsafe
         .replace(/&/g, "&amp;")
         .replace(/</g, "&lt;")
         .replace(/>/g, "&gt;")
         .replace(/"/g, "&quot;")
         .replace(/'/g, "&#039;");
}
