# Progress

- 현재 상태: `DOCUMENTING`
- 마지막 업데이트: `2026-04-27`
- 현재 단계: 문서화 완료 및 사용자 공유 대기

## 현재 스냅샷

- Release x86 빌드 성공
- COM 객체 생성 확인 성공 (`HWPFrame.HwpObject`)
- `test.hwp` 변환 검증:
  - 성공: `PDF`, `HWPX`, `HWPML2X`, `HTML+`, `OOXML`, `UNICODE`, `RTF`
  - 스킵: `HWP`
  - 실패: `ODT`
- old GUI vs 현재 CLI 비교 분석 완료:
  - old GUI: `AxInterop/Interop.HWPCONTROLLib` 기반 `AxHwpCtrl`
  - 현재 코드: `HwpObjectLib` 기반 `HWPFrame.HwpObject`
  - 결론: 인터롭 DLL만으로 무설정 런타임 대체 불가
