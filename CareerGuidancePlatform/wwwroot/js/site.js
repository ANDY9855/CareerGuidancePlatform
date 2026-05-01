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
