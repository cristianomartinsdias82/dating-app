export interface Message {
  id: string,
  senderId: string,
  senderUserName: string,
  senderPhotoUrl: string,
  senderKnownAs: string,
  recipientId: string,
  recipientUserName: string,
  recipientKnownAs: string,
  recipientPhotoUrl: string,
  content: string,
  dateSent: Date,
  dateRead?: Date
}
